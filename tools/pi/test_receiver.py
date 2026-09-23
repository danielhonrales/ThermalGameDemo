import json
import unittest
from receiver import Receiver


class ReceiverChecks(unittest.TestCase):
    def packet(self, seq, session='quest-a', event='snapshot'):
        return json.dumps(dict(schema=1, session=session, device=session, seq=seq,
                               event=event, active=['shield'])).encode()

    def test_two_headsets_and_reordering(self):
        r = Receiver()
        self.assertIsNotNone(r.accept(self.packet(5), 0))
        self.assertIsNone(r.accept(self.packet(5), 0.1))
        self.assertIsNone(r.accept(self.packet(4), 0.2))
        self.assertIsNotNone(r.accept(self.packet(1, 'quest-b'), 0.2))
        self.assertEqual(len(r.sessions), 2)

    def test_timeout_once_then_recovery(self):
        r = Receiver()
        r.accept(self.packet(1), 0)
        self.assertEqual(r.expire(0.9), [])
        self.assertEqual(r.expire(1.01)[0]['event'], 'output_timeout')
        self.assertEqual(r.expire(1.5), [])
        self.assertIsNotNone(r.accept(self.packet(2), 2))
        self.assertEqual(r.expire(2.9), [])
        self.assertEqual(len(r.expire(3.1)), 1)

    def test_bad_packets_do_not_break_listener(self):
        r = Receiver()
        for packet in (b'', b'garbage', b'[]', b'null', b'{}', b'{"schema":2}'):
            self.assertIsNone(r.accept(packet, 0))
        self.assertIsNotNone(r.accept(self.packet(1), 0))


if __name__ == '__main__':
    unittest.main()
