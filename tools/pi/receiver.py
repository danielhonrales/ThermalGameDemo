#!/usr/bin/env python3
"""Dummy LAN receiver: prints only; no GPIO, heaters, motors or actuators."""
import argparse
import json
import socket
import time

SIGNALS = {'ice_shot', 'fire_start', 'fire_stop', 'hit_received', 'shield_block'}


class Receiver:
    def __init__(self):
        self.sessions = {}

    def accept(self, payload, now):
        try:
            msg = json.loads(payload)
            if (msg.get('schema') != 1 or not isinstance(msg.get('session'), str)
                    or not isinstance(msg.get('seq'), int) or not isinstance(msg.get('event'), str)):
                return None
            key = msg['session']
            old = self.sessions.get(key)
            if old and msg['seq'] <= old['seq']:
                return None  # Ignore duplicates and reordered datagrams.
            self.sessions[key] = dict(seq=msg['seq'], seen=now, expired=False, device=msg.get('device'))
            return msg
        except (ValueError, TypeError, AttributeError):
            return None

    def expire(self, now):
        expired = []
        for key, state in list(self.sessions.items()):
            if now - state['seen'] > 1.0 and not state['expired']:
                state['expired'] = True
                expired.append(dict(session=key, device=state['device'], event='output_timeout',
                                    active=[]))
            if now - state['seen'] > 60:
                del self.sessions[key]
        return expired


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--bind', default='0.0.0.0')
    parser.add_argument('--port', type=int, default=7779)
    args = parser.parse_args()
    receiver = Receiver()
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as sock:
        sock.bind((args.bind, args.port))
        sock.settimeout(0.1)
        print(f'Dummy receiver listening on {args.bind}:{args.port}', flush=True)
        while True:
            try:
                payload, address = sock.recvfrom(4096)
                msg = receiver.accept(payload, time.monotonic())
                if msg and msg['event'] in SIGNALS:
                    msg['from'] = address[0]
                    print(json.dumps(msg, separators=(',', ':')), flush=True)
            except socket.timeout:
                pass
            for msg in receiver.expire(time.monotonic()):
                print(json.dumps(msg, separators=(',', ':')), flush=True)


if __name__ == '__main__':
    main()
