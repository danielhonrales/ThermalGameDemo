"""Flashpoint postcard booth: photograph the winner and loser of a match and make a shareable postcard.

Run:   python tools/postcard/postcard_app.py [--camera N]   (default: the built-in webcam, 0)
Test:  python tools/postcard/postcard_app.py --render WINNER.jpg LOSER.jpg --names Ana Ben

The winner's photo gets a crown, the loser's gets tears (both placed with OpenCV face detection; they
fall back to the photo's centre if no face is found). Postcards are saved to tools/postcard/output.
"""
import argparse
import math
import os
import random
import re
import sys
import time
import tkinter as tk
from datetime import datetime
from functools import lru_cache
from pathlib import Path

import cv2
import numpy as np
from PIL import Image, ImageDraw, ImageEnhance, ImageFilter, ImageFont, ImageOps, ImageTk

HERE = Path(__file__).resolve().parent
ASSETS = HERE / "assets"
OUTPUT = HERE / "output"

CARD_W, CARD_H = 1920, 1080
PORTRAIT = 380                      # portrait circle diameter on the card
WINNER_POS = (600, 530)
LOSER_POS = (1320, 530)
FIRE = (255, 122, 40)
FIRE_LIGHT = (255, 196, 92)
ICE = (60, 170, 255)
ICE_LIGHT = (150, 220, 255)

# ---------------------------------------------------------------- fonts

_FONT_CACHE = {}


def font(size, weight="Bold"):
    key = (size, weight)
    if key not in _FONT_CACHE:
        f = None
        try:
            f = ImageFont.truetype("bahnschrift.ttf", size)
            f.set_variation_by_name(weight)
        except Exception:
            try:
                f = ImageFont.truetype("segoeuib.ttf", size)
            except OSError:
                f = ImageFont.load_default()
        _FONT_CACHE[key] = f
    return _FONT_CACHE[key]


# ---------------------------------------------------------------- face detection

_FACE = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")
_EYE = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_eye.xml")


def detect_face(img, min_px=60):
    """Largest face in a PIL image as {'box': (x, y, w, h), 'eyes': [(cx, cy, size), ...]}, or None."""
    gray = cv2.equalizeHist(cv2.cvtColor(np.asarray(img.convert("RGB")), cv2.COLOR_RGB2GRAY))
    min_side = max(min_px, min(gray.shape) // 8)
    faces = _FACE.detectMultiScale(gray, scaleFactor=1.1, minNeighbors=6, minSize=(min_side, min_side))
    if len(faces) == 0:
        return None
    x, y, w, h = (int(v) for v in max(faces, key=lambda f: f[2] * f[3]))
    # Eyes: search the upper part of the face; keep one per side.
    roi = gray[y + h // 6: y + h // 2 + h // 10, x: x + w]
    found = _EYE.detectMultiScale(roi, scaleFactor=1.1, minNeighbors=5, minSize=(w // 8, w // 8))
    eyes = []
    for side in (0, 1):
        candidates = [e for e in found if ((e[0] + e[2] / 2) > w / 2) == bool(side)]
        if candidates:
            ex, ey, ew, eh = max(candidates, key=lambda e: e[2])
            eyes.append((x + ex + ew / 2, y + h // 6 + ey + eh / 2, ew))
    if len(eyes) != 2:   # typical eye positions within a detected face box
        eyes = [(x + w * 0.31, y + h * 0.40, w * 0.2), (x + w * 0.69, y + h * 0.40, w * 0.2)]
    return {"box": (x, y, w, h), "eyes": eyes}


def portrait_crop(img, face):
    """Square crop centred on the face with head room for the crown: (left, top, side)."""
    W, H = img.size
    if face:
        x, y, w, h = face["box"]
        side = min(int(w * 2.1), W, H)
        cx, cy = x + w / 2, y + h * 0.58
    else:
        side = min(W, H)
        cx, cy = W / 2, H / 2
    left = int(min(max(cx - side / 2, 0), W - side))
    top = int(min(max(cy - side / 2, 0), H - side))
    return left, top, side


# ---------------------------------------------------------------- effects

def crown_image(width):
    """A gold five-point crown with jewels, RGBA, about 0.72 * width tall."""
    s = 4                                   # supersampling
    W, H = width * s, int(width * 0.72) * s
    pad = int(0.06 * W)
    img = Image.new("RGBA", (W + 2 * pad, H + 2 * pad), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    ox, oy = pad, pad
    tip_y = [0.22, 0.10, 0.0, 0.10, 0.22]
    tip_x = [0.02, 0.26, 0.5, 0.74, 0.98]
    valley_y = 0.48
    base_top, base_bot = 0.66, 0.97
    pts = [(ox + 0.06 * W, oy + base_bot * H)]
    pts.append((ox + tip_x[0] * W, oy + tip_y[0] * H))
    for i in range(1, 5):
        pts.append((ox + (tip_x[i - 1] + tip_x[i]) / 2 * W, oy + valley_y * H))
        pts.append((ox + tip_x[i] * W, oy + tip_y[i] * H))
    pts.append((ox + 0.94 * W, oy + base_bot * H))

    # Vertical gold gradient clipped to the crown shape.
    mask = Image.new("L", img.size, 0)
    ImageDraw.Draw(mask).polygon(pts, fill=255)
    grad = np.zeros((img.height, img.width, 4), np.uint8)
    t = np.linspace(0, 1, img.height)[:, None]
    top_c, bot_c = np.array([255, 236, 140]), np.array([214, 140, 22])
    grad[..., :3] = (top_c * (1 - t) + bot_c * t)[:, None, :].astype(np.uint8)
    grad[..., 3] = 255
    gold = Image.fromarray(grad)
    img.paste(gold, (0, 0), mask)
    outline = (120, 72, 0, 255)
    d.line(pts + [pts[0]], fill=outline, width=int(0.022 * W), joint="curve")
    # Band across the base.
    d.rectangle((ox + 0.05 * W, oy + base_top * H, ox + 0.95 * W, oy + base_top * H + 0.05 * H), fill=(150, 92, 6, 255))
    # Balls on the tips.
    r = 0.055 * W
    for tx, ty in zip(tip_x, tip_y):
        cx, cy = ox + tx * W, oy + ty * H
        d.ellipse((cx - r, cy - r, cx + r, cy + r), fill=(255, 226, 120, 255), outline=outline, width=int(0.014 * W))
        d.ellipse((cx - r * 0.45, cy - r * 0.55, cx - r * 0.05, cy - r * 0.15), fill=(255, 250, 220, 255))
    # Jewels on the band.
    jy = oy + (base_top + 0.17) * H
    for jx, color, jr in ((0.5, (220, 30, 50), 0.075), (0.25, (40, 120, 230), 0.055), (0.75, (40, 120, 230), 0.055)):
        cx, rr = ox + jx * W, jr * W
        d.ellipse((cx - rr, jy - rr, cx + rr, jy + rr), fill=color + (255,), outline=outline, width=int(0.012 * W))
        d.ellipse((cx - rr * 0.5, jy - rr * 0.6, cx - rr * 0.05, jy - rr * 0.2), fill=(255, 255, 255, 200))
    return img.resize((img.width // s, img.height // s), Image.LANCZOS)


def teardrop(d, cx, cy, r, fill, outline, width):
    """Teardrop: a circle of radius r at (cx, cy) tapering to a point straight above it."""
    def shape(rr, fill_color):
        apex = cy - rr * 2.1
        L = cy - apex
        tx, ty = rr * math.sqrt(1 - (rr / L) ** 2), cy - rr * rr / L   # where the sides touch the circle
        d.polygon([(cx, apex), (cx - tx, ty), (cx + tx, ty)], fill=fill_color)
        d.ellipse((cx - rr, cy - rr, cx + rr, cy + rr), fill=fill_color)
    shape(r + width, outline)     # outline: the same shape, slightly larger, underneath
    shape(r, fill)
    d.ellipse((cx - r * 0.55, cy - r * 0.5, cx - r * 0.15, cy - r * 0.05), fill=(255, 255, 255, 220))


@lru_cache(maxsize=64)
def tilted_crown(width):
    crown = crown_image(width).rotate(-12, resample=Image.BICUBIC, expand=True)
    shadow = Image.new("RGBA", crown.size, (0, 0, 0, 0))
    shadow.putalpha(crown.getchannel("A").point(lambda a: a * 0.5))
    return crown, shadow.filter(ImageFilter.GaussianBlur(max(2, width / 30)))


def paste_crown(target, fx, fy, fw, fh):
    """Crown sitting on the head of the face box (fx, fy, fw, fh), tilted, with a drop shadow."""
    crown, shadow = tilted_crown(max(16, int(round(fw / 4) * 4)))
    x = int(fx + fw * 0.52 - crown.width / 2)
    y = int(fy - fh * 0.12 - crown.height * 0.88)
    off = max(2, int(fw * 0.04))
    target.paste(shadow, (x + off, y + off), shadow)
    target.paste(crown, (x, y), crown)


def draw_tears(img, eyes, scale, s=3):
    """Draw tear streams and falling drops under each eye (eye coords in img space); s is supersampling."""
    layer = Image.new("RGBA", (img.width * s, img.height * s), (0, 0, 0, 0))
    d = ImageDraw.Draw(layer)
    for ex, ey, size in eyes:
        ex, ey, size = ex * s, ey * s, max(size * s, 12 * s * scale)
        # Stream from the lower eyelid down the cheek, wobbling slightly outward.
        outward = -1 if ex < layer.width / 2 else 1
        stream = []
        length = size * 1.9
        for i in range(25):
            t = i / 24
            stream.append((ex + outward * size * 0.15 * t + math.sin(t * 7) * size * 0.05,
                           ey + size * 0.3 + length * t))
        w = int(size * 0.34)
        d.line(stream, fill=(30, 110, 200, 200), width=w + max(2, int(size * 0.07)), joint="curve")
        d.line(stream, fill=(130, 205, 255, 230), width=w, joint="curve")
        d.line([(x - w * 0.18, y) for x, y in stream[2:-3]], fill=(240, 250, 255, 230), width=max(1, w // 4))
        # Drops gathering at the end and falling below.
        bx, by = stream[-1]
        teardrop(d, bx, by + size * 0.2, size * 0.36, (150, 215, 255, 245), (30, 110, 200, 255), max(2, int(size * 0.05)))
        teardrop(d, bx + outward * size * 0.15, by + size * 1.05, size * 0.26, (150, 215, 255, 235), (30, 110, 200, 255), max(2, int(size * 0.045)))
    if s != 1:
        layer = layer.resize(img.size, Image.LANCZOS)
    out = img.convert("RGBA")
    out.alpha_composite(layer)
    return out.convert(img.mode)


def tint(img, color, amount, saturation=1.0):
    if saturation != 1.0:
        img = ImageEnhance.Color(img).enhance(saturation)
    overlay = Image.new("RGB", img.size, color)
    return Image.blend(img, overlay, amount)


def prepare_player(photo, role):
    """Detect the face once; returns what draw_player needs."""
    photo = ImageOps.exif_transpose(photo).convert("RGB")
    return {"photo": photo, "face": detect_face(photo), "role": role}


def draw_player(card, center, diameter, player):
    """Draw a framed portrait with its effect onto an RGBA card, centred at `center`."""
    photo, face, role = player["photo"], player["face"], player["role"]
    winner = role == "winner"
    left, top, side = portrait_crop(photo, face)
    k = diameter / side
    portrait = photo.crop((left, top, left + side, top + side)).resize((diameter, diameter), Image.LANCZOS)
    portrait = tint(portrait, (255, 150, 60), 0.08, 1.08) if winner else tint(portrait, (70, 140, 255), 0.12, 0.8)
    if face:
        x, y, w, h = face["box"]
        fx, fy, fw, fh = (x - left) * k, (y - top) * k, w * k, h * k
        eyes = [((ex - left) * k, (ey - top) * k, es * k) for ex, ey, es in face["eyes"]]
    else:   # assume a centred head
        fw = fh = diameter * 0.45
        fx, fy = (diameter - fw) / 2, diameter * 0.3
        eyes = [(fx + fw * 0.31, fy + fh * 0.4, fw * 0.2), (fx + fw * 0.69, fy + fh * 0.4, fw * 0.2)]
    if not winner:
        portrait = draw_tears(portrait, eyes, diameter / 400)

    color, light = (FIRE, FIRE_LIGHT) if winner else (ICE, ICE_LIGHT)
    cx, cy = center
    r = diameter // 2
    # Glow behind the frame.
    glow = Image.new("RGBA", card.size, (0, 0, 0, 0))
    ImageDraw.Draw(glow).ellipse((cx - r - 26, cy - r - 26, cx + r + 26, cy + r + 26), fill=color + (210,))
    card.alpha_composite(glow.filter(ImageFilter.GaussianBlur(28)))
    # Circular photo.
    s = 4
    mask = Image.new("L", (diameter * s, diameter * s), 0)
    ImageDraw.Draw(mask).ellipse((0, 0, diameter * s - 1, diameter * s - 1), fill=255)
    mask = mask.resize((diameter, diameter), Image.LANCZOS)
    card.paste(portrait, (cx - r, cy - r), mask)
    ring = Image.new("RGBA", (diameter * s + 40 * s, diameter * s + 40 * s), (0, 0, 0, 0))
    rd = ImageDraw.Draw(ring)
    o = 20 * s
    rd.ellipse((o - 5 * s, o - 5 * s, o + diameter * s + 5 * s, o + diameter * s + 5 * s), outline=light + (255,), width=9 * s)
    rd.ellipse((o - 5 * s, o - 5 * s, o + diameter * s + 5 * s, o + diameter * s + 5 * s), outline=(255, 255, 255, 140), width=2 * s)
    ring = ring.resize((ring.width // s, ring.height // s), Image.LANCZOS)
    card.alpha_composite(ring, (cx - r - 20, cy - r - 20))

    if winner:   # allowed to rise past the frame
        paste_crown(card, cx - r + fx, cy - r + fy, fw, fh)

    # Label pill over the bottom of the frame.
    label = "WINNER" if winner else "DEFEATED"
    f = font(34, "Bold")
    tw = f.getlength(label)
    pw, ph = int(tw + 64), 58
    px, py = cx - pw // 2, cy + r - ph // 2 + 6
    pill = Image.new("RGBA", (pw * s, ph * s), (0, 0, 0, 0))
    pd = ImageDraw.Draw(pill)
    pd.rounded_rectangle((0, 0, pw * s - 1, ph * s - 1), radius=ph * s // 2, fill=color + (255,), outline=(255, 255, 255, 230), width=3 * s)
    pill = pill.resize((pw, ph), Image.LANCZOS)
    card.alpha_composite(pill, (px, py))
    d = ImageDraw.Draw(card)
    text_color = (40, 14, 0) if winner else (2, 20, 48)
    d.text((cx, py + ph / 2 + 1), label, font=f, fill=text_color, anchor="mm")


# ---------------------------------------------------------------- card

def background():
    """Fire on the left, ice on the right, dark in the middle, with sparks and ice motes."""
    xs = np.linspace(0, 1, CARD_W)[None, :, None]
    ys = np.linspace(0, 1, CARD_H)[:, None, None]
    left, mid, right = np.array([70, 14, 6]), np.array([10, 10, 22]), np.array([6, 24, 70])
    t = xs
    base = np.where(t < 0.5, left * (1 - t * 2) + mid * (t * 2), mid * (2 - t * 2) + right * (t * 2 - 1))
    img = np.broadcast_to(base, (CARD_H, CARD_W, 3)).astype(float).copy()

    def glow(px, py, sx, sy, color, strength):
        g = np.exp(-(((xs - px) / sx) ** 2 + ((ys - py) / sy) ** 2))
        return g * np.array(color) * strength

    img += glow(0.10, 0.55, 0.28, 0.55, (255, 90, 20), 0.75)
    img += glow(0.28, 0.48, 0.16, 0.30, (255, 140, 40), 0.30)
    img += glow(0.90, 0.55, 0.28, 0.55, (30, 130, 255), 0.75)
    img += glow(0.72, 0.48, 0.16, 0.30, (90, 200, 255), 0.25)
    vignette = 1 - 0.55 * (((xs - 0.5) * 1.6) ** 2 + ((ys - 0.5) * 1.9) ** 2)
    img = np.clip(img * np.clip(vignette, 0.25, 1), 0, 255).astype(np.uint8)
    card = Image.fromarray(img).convert("RGBA")

    rng = random.Random(7)
    motes = Image.new("RGBA", card.size, (0, 0, 0, 0))
    d = ImageDraw.Draw(motes)
    for _ in range(260):
        fire = rng.random() < 0.5
        x = rng.betavariate(1, 2.6) * CARD_W * 0.6 if fire else CARD_W - rng.betavariate(1, 2.6) * CARD_W * 0.6
        y = rng.uniform(0, CARD_H)
        r = rng.choice([1, 1, 1.5, 2, 2.5, 3.5])
        c = (rng.choice([FIRE_LIGHT, (255, 230, 150), FIRE]) if fire else rng.choice([ICE_LIGHT, (230, 248, 255), ICE]))
        d.ellipse((x - r, y - r, x + r, y + r), fill=c + (rng.randint(120, 255),))
    card.alpha_composite(motes.filter(ImageFilter.GaussianBlur(3)))
    card.alpha_composite(motes)
    return card


def paste_logo(card, path, height, x, y, anchor):
    logo = Image.open(path).convert("RGBA")
    logo = logo.resize((int(logo.width * height / logo.height), height), Image.LANCZOS)
    if anchor == "right":
        x -= logo.width
    card.alpha_composite(logo, (int(x), int(y - height / 2)))


def fit_font(text_runs, max_width, size, weight="Bold"):
    while size > 30:
        f = font(size, weight)
        if sum(f.getlength(t) for t, _ in text_runs) <= max_width:
            return f
        size -= 4
    return font(size, weight)


def draw_runs(card, runs, cx, y, f, glow=True):
    """Centre coloured text runs on one line, with a dark shadow for legibility."""
    total = sum(f.getlength(t) for t, _ in runs)
    shadow = Image.new("RGBA", card.size, (0, 0, 0, 0))
    sd = ImageDraw.Draw(shadow)
    x = cx - total / 2
    for text, _ in runs:
        sd.text((x, y + 4), text, font=f, fill=(0, 0, 0, 230), anchor="ls")
        x += f.getlength(text)
    card.alpha_composite(shadow.filter(ImageFilter.GaussianBlur(9 if glow else 4)))
    d = ImageDraw.Draw(card)
    x = cx - total / 2
    for text, color in runs:
        d.text((x, y), text, font=f, fill=color, anchor="ls")
        x += f.getlength(text)


def make_postcard(winner_name, loser_name, winner, loser):
    card = background()

    title = Image.open(ASSETS / "flashpoint_title.png").convert("RGBA")
    tw = 860
    title = title.resize((tw, int(title.height * tw / title.width)), Image.LANCZOS)
    tshadow = Image.new("RGBA", card.size, (0, 0, 0, 0))
    ImageDraw.Draw(tshadow).rounded_rectangle(((CARD_W - tw) // 2, 40, (CARD_W + tw) // 2, 40 + title.height), radius=30, fill=(0, 0, 0, 200))
    card.alpha_composite(tshadow.filter(ImageFilter.GaussianBlur(16)))
    card.alpha_composite(title, ((CARD_W - tw) // 2, 30))

    draw_player(card, LOSER_POS, PORTRAIT, loser)
    draw_player(card, WINNER_POS, PORTRAIT, winner)

    runs = [(winner_name, FIRE_LIGHT), (" beat ", (255, 255, 255)), (loser_name, ICE_LIGHT)]
    draw_runs(card, runs, CARD_W / 2, 862, fit_font(runs, 1640, 92))
    draw_runs(card, [("in ", (225, 225, 235)), ("Flashpoint", (255, 255, 255)), (" at ", (225, 225, 235)), ("UIST 2026", (255, 255, 255))],
              CARD_W / 2, 925, font(46, "SemiBold"), glow=False)

    # Bottom band with the lab logo on the left and the conference logo on the right.
    band_top = 962
    band = Image.new("RGBA", card.size, (0, 0, 0, 0))
    bd = ImageDraw.Draw(band)
    bd.rectangle((0, band_top, CARD_W, CARD_H), fill=(6, 8, 16, 215))
    for i in range(CARD_W):   # fire-to-ice rule along the top edge
        t = i / CARD_W
        c = tuple(int(a * (1 - t) + b * t) for a, b in zip(FIRE, ICE))
        bd.line((i, band_top, i, band_top + 3), fill=c + (255,))
    card.alpha_composite(band)
    mid = (band_top + CARD_H) / 2 + 2
    paste_logo(card, ASSETS / "milab_logo.png", 54, 64, mid, "left")
    # The UIST logo's teal and gold are made for a light background, so give it a plate.
    plate_h, logo_h = 78, 46
    logo = Image.open(ASSETS / "uist2026_logo.png")
    plate_w = int(logo.width * logo_h / logo.height) + 52
    plate = Image.new("RGBA", (plate_w * 4, plate_h * 4), (0, 0, 0, 0))
    ImageDraw.Draw(plate).rounded_rectangle((0, 0, plate_w * 4 - 1, plate_h * 4 - 1), radius=16 * 4, fill=(248, 245, 236, 255))
    card.alpha_composite(plate.resize((plate_w, plate_h), Image.LANCZOS), (CARD_W - 56 - plate_w, int(mid - plate_h / 2)))
    paste_logo(card, ASSETS / "uist2026_logo.png", logo_h, CARD_W - 56 - 26, mid, "right")
    return card.convert("RGB")


def save_postcard(card, winner_name, loser_name):
    OUTPUT.mkdir(exist_ok=True)
    slug = lambda s: re.sub(r"[^A-Za-z0-9]+", "-", s).strip("-")[:24] or "player"
    path = OUTPUT / f"flashpoint_{datetime.now():%Y%m%d_%H%M%S}_{slug(winner_name)}_beat_{slug(loser_name)}.png"
    card.save(path)
    return path


# ---------------------------------------------------------------- app

BG = "#0b0d16"
PANEL = "#151927"
PANEL_RGB = (21, 25, 39)
TEXT = "#e8eaf2"
MUTED = "#9aa3b8"
ROLE_STYLE = {"winner": ("WINNER", "#ffb35c", FIRE), "loser": ("LOSER", "#7fd0ff", ICE)}


class BoothApp:
    def __init__(self, root, camera_index):
        self.root = root
        root.title("Flashpoint Postcard Booth")
        root.configure(bg=BG)
        root.geometry("1400x860")
        root.minsize(1000, 640)
        root.attributes("-fullscreen", True)

        self.cap = cv2.VideoCapture(camera_index, cv2.CAP_DSHOW)
        if self.cap.isOpened():
            self.cap.set(cv2.CAP_PROP_FRAME_WIDTH, 1280)
            self.cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 720)
        self.frame = None            # latest camera frame (PIL, mirrored)
        self.live_face = None        # smoothed face in the live feed, in 0..1 frame units
        self.face_missing = 0
        self.countdown_end = None
        self.flash_until = 0
        self.players = {}            # role -> prepared player
        self.previews = {}           # role -> framed portrait shown after the photo
        self.thumbs = {}             # role -> PhotoImage for the match panel
        self.names = {}
        self.step = "winner"
        self.card = None
        self.card_path = None
        self.card_view_key = None
        self.menu_visible = True
        self.fullscreen = None
        self._photo_ref = None
        self._shown_step = None

        # The panel is packed first so the camera view can never squeeze it off screen.
        self.panel = panel = tk.Frame(root, bg=PANEL, width=360)
        self.panel_pack = dict(side="right", fill="y", padx=(8, 16), pady=16)
        panel.pack(**self.panel_pack)
        panel.pack_propagate(False)
        self.view_box = tk.Frame(root, bg=BG)
        self.view_box.pack(side="left", fill="both", expand=True, padx=(16, 8), pady=16)
        self.view = tk.Label(self.view_box, bg=BG)
        self.view.place(relx=0.5, rely=0.5, anchor="center")
        self.view.bind("<Button-1>", lambda e: self.toggle_menu())

        # Match: who is the winner and who is the loser, filled in as photos are taken.
        tk.Label(panel, text="MATCH", font=("Bahnschrift", 12, "bold"), fg=MUTED, bg=PANEL).pack(anchor="w", padx=24, pady=(22, 6))
        self.slots = {}
        blank = Image.new("RGB", (72, 72), PANEL_RGB)
        ImageDraw.Draw(blank).ellipse((2, 2, 69, 69), fill=(34, 39, 58))
        ImageDraw.Draw(blank).text((36, 37), "?", font=font(34), fill=(110, 118, 140), anchor="mm")
        self.blank_thumb = ImageTk.PhotoImage(blank)
        for role in ("winner", "loser"):
            title, fg, _ = ROLE_STYLE[role]
            row = tk.Frame(panel, bg=PANEL, highlightthickness=2, highlightbackground=PANEL)
            row.pack(fill="x", padx=18, pady=3)
            pic = tk.Label(row, image=self.blank_thumb, bg=PANEL)
            pic.pack(side="left", padx=6, pady=6)
            tk.Label(row, text=title, font=("Bahnschrift", 12, "bold"), fg=fg, bg=PANEL).pack(anchor="w", padx=6, pady=(12, 0))
            name = tk.Label(row, text="", font=("Bahnschrift", 17), fg=TEXT, bg=PANEL, anchor="w")
            name.pack(anchor="w", padx=6)
            self.slots[role] = (row, pic, name)
        tk.Frame(panel, bg="#2a3045", height=1).pack(fill="x", padx=24, pady=(14, 0))

        self.step_label = tk.Label(panel, font=("Bahnschrift", 30, "bold"), bg=PANEL)
        self.step_label.pack(anchor="w", padx=24, pady=(14, 2))
        self.hint = tk.Label(panel, font=("Bahnschrift", 13), fg=MUTED, bg=PANEL, justify="left", wraplength=310)
        self.hint.pack(anchor="w", padx=24, pady=(0, 16))

        self.name_label = tk.Label(panel, text="Name", font=("Bahnschrift", 13), fg=TEXT, bg=PANEL)
        self.name_var = tk.StringVar()
        self.name_var.trace_add("write", lambda *a: self.refresh_slots())
        self.name_entry = tk.Entry(panel, textvariable=self.name_var, font=("Bahnschrift", 20), bg="#0e111b", fg="white",
                                   insertbackground="white", relief="flat", highlightthickness=2, highlightcolor="#4a90ff",
                                   highlightbackground="#2a3045")
        self.name_entry.bind("<Return>", lambda e: self.primary())

        def button(text, cmd, bg="#2a3045"):
            return tk.Button(panel, text=text, command=cmd, font=("Bahnschrift", 15, "bold"), bg=bg, fg="white",
                             activebackground="#3a4260", activeforeground="white", relief="flat", cursor="hand2", pady=10)

        self.capture_btn = button("Take photo", self.start_countdown, "#d1561e")
        self.retake_btn = button("Retake", self.retake)
        self.next_btn = button("Next", self.next_step, "#2f6fdb")
        self.back_btn = button("Back", self.back)
        self.full_btn = button("Full screen (F11)", self.show_fullscreen, "#2f6fdb")
        self.new_btn = button("New match", self.new_match, "#d1561e")
        self.hide_btn = button("Hide menu", self.toggle_menu)
        self.folder_btn = button("Open postcards folder", lambda: os.startfile(OUTPUT))
        self.status = tk.Label(panel, font=("Bahnschrift", 12), fg=MUTED, bg=PANEL, justify="left", wraplength=310)

        root.bind("<F11>", lambda e: self.show_fullscreen())
        root.bind("<Escape>", lambda e: self.escape())
        root.protocol("WM_DELETE_WINDOW", self.quit)
        self.refresh_panel()
        self.tick()

    # ---- state

    def refresh_slots(self):
        for role, (row, pic, name) in self.slots.items():
            current = role == self.step
            text = self.name_var.get().strip() if current else self.names.get(role, "")
            name.config(text=text or ("(type name)" if current else ""), fg=TEXT if text else MUTED)
            pic.config(image=self.thumbs.get(role, self.blank_thumb))
            row.config(highlightbackground=ROLE_STYLE[role][1] if current else PANEL)

    def refresh_panel(self):
        for w in (self.capture_btn, self.retake_btn, self.next_btn, self.back_btn, self.full_btn, self.new_btn, self.hide_btn,
                  self.folder_btn, self.status, self.name_label, self.name_entry):
            w.pack_forget()
        if self.step == "card":
            self._shown_step = self.step
            self.step_label.config(text="POSTCARD", fg="white")
            self.hint.config(text="Click the postcard to show or hide this menu.")
            for w in (self.full_btn, self.new_btn, self.back_btn, self.hide_btn, self.folder_btn):
                w.pack(fill="x", padx=24, pady=5)
            self.status.config(text=f"Saved: {self.card_path.name}" if self.card_path else "", fg=MUTED)
            self.status.pack(anchor="w", padx=24, pady=(14, 0))
            self.refresh_slots()
            return
        winner = self.step == "winner"
        title, fg, _ = ROLE_STYLE[self.step]
        self.step_label.config(text=title, fg=fg)
        self.hint.config(text=("Step 1 of 2. Type the winner's name, line them up so the crown sits on their head, "
                               "then take the photo." if winner else
                               "Step 2 of 2. Type the loser's name, line them up with the tears, then take the photo."))
        self.name_label.pack(anchor="w", padx=24)
        self.name_entry.pack(fill="x", padx=24, pady=(4, 16), ipady=6)
        if self.step != self._shown_step:   # keep what's typed when only the buttons change
            self.name_var.set(self.names.get(self.step, ""))
        self._shown_step = self.step
        self.name_entry.focus_set()
        if self.step in self.players:
            self.retake_btn.pack(fill="x", padx=24, pady=5)
            self.next_btn.config(text="Next: loser" if winner else "Make postcard")
            self.next_btn.pack(fill="x", padx=24, pady=5)
        else:
            self.capture_btn.pack(fill="x", padx=24, pady=5)
        if not winner:
            self.back_btn.pack(fill="x", padx=24, pady=5)
        self.status.config(text="Enter: take photo / continue" if self.cap.isOpened() else "Camera not found. Try --camera 1.",
                           fg=MUTED)
        self.status.pack(anchor="w", padx=24, pady=(14, 0))
        self.refresh_slots()

    def set_menu(self, visible):
        if visible and not self.menu_visible:
            self.panel.pack(before=self.view_box, **self.panel_pack)
        elif not visible and self.menu_visible:
            self.panel.pack_forget()
        self.menu_visible = visible

    def toggle_menu(self):
        if self.step == "card":   # the menu always stays up while taking photos
            self.set_menu(not self.menu_visible)

    def primary(self):
        if self.step == "card":
            return
        if self.step in self.players:
            self.next_step()
        else:
            self.start_countdown()

    def start_countdown(self):
        if self.frame is None or self.countdown_end:
            return
        self.countdown_end = time.time() + 3

    def capture(self):
        role = self.step
        player = prepare_player(self.frame.copy(), role)
        self.players[role] = player
        self.previews[role] = self.player_preview(player)
        self.thumbs[role] = ImageTk.PhotoImage(self.previews[role].crop((130, 90, 630, 590)).resize((72, 72), Image.LANCZOS))
        self.flash_until = time.time() + 0.25
        self.refresh_panel()

    def retake(self):
        for d in (self.players, self.previews, self.thumbs):
            d.pop(self.step, None)
        self.live_face = None
        self.refresh_panel()

    def next_step(self):
        name = self.name_var.get().strip()
        if not name:
            self.status.config(text="Please enter a name first.", fg="#ff8a80")
            self.name_entry.focus_set()
            return
        if self.step not in self.players:
            self.start_countdown()
            return
        self.names[self.step] = name
        if self.step == "winner":
            self.step = "loser"
            self.live_face = None
        else:
            self.card = make_postcard(self.names["winner"], self.names["loser"], self.players["winner"], self.players["loser"])
            self.card_path = save_postcard(self.card, self.names["winner"], self.names["loser"])
            self.step = "card"
            self.set_menu(False)   # let the postcard fill the window
        self.refresh_panel()

    def back(self):
        if self.step == "card":
            self.step = "loser"
            self.set_menu(True)
        elif self.step == "loser":
            self.names["loser"] = self.name_var.get().strip()
            self.step = "winner"
        self.refresh_panel()

    def new_match(self):
        for d in (self.players, self.previews, self.thumbs, self.names):
            d.clear()
        self.card = self.card_path = None
        self.live_face = None
        self.step = "winner"
        self.set_menu(True)
        self.refresh_panel()

    # ---- display

    def show_fullscreen(self):
        if self.card is None or self.fullscreen:
            return
        top = tk.Toplevel(self.root, bg="black")
        top.attributes("-fullscreen", True)
        sw, sh = top.winfo_screenwidth(), top.winfo_screenheight()
        img = ImageOps.contain(self.card, (sw, sh), Image.LANCZOS)
        top._img = ImageTk.PhotoImage(img)
        tk.Label(top, image=top._img, bg="black").pack(expand=True)
        for seq in ("<Escape>", "<F11>", "<Button-1>"):
            top.bind(seq, lambda e: self.close_fullscreen())
        top.focus_force()
        self.fullscreen = top

    def escape(self):
        """Esc closes the full-screen postcard if it's up, otherwise toggles the booth window's full screen."""
        if self.fullscreen:
            self.close_fullscreen()
        else:
            self.root.attributes("-fullscreen", not self.root.attributes("-fullscreen"))

    def close_fullscreen(self):
        if self.fullscreen:
            self.fullscreen.destroy()
            self.fullscreen = None

    def player_preview(self, player):
        size = 760
        canvas = Image.new("RGBA", (size, size), PANEL_RGB + (255,))
        draw_player(canvas, (size // 2, size // 2 + 50), 440, player)
        return canvas.convert("RGB")

    def track_face(self):
        """Detect the face on a small copy of the frame and smooth it, so the live effect follows without jitter."""
        small = self.frame.resize((480, round(480 * self.frame.height / self.frame.width)), Image.BILINEAR)
        found = detect_face(small, min_px=30)
        if not found:
            self.face_missing += 1
            if self.face_missing > 8:
                self.live_face = None
            return
        self.face_missing = 0
        sw, sh = small.size
        x, y, w, h = found["box"]
        face = [x / sw, y / sh, w / sw, h / sh] + [v for ex, ey, es in found["eyes"] for v in (ex / sw, ey / sh, es / sw)]
        prev = self.live_face
        if prev and abs(prev[0] - face[0]) < 0.15 and abs(prev[1] - face[1]) < 0.15:
            face = [p * 0.55 + f * 0.45 for p, f in zip(prev, face)]
        self.live_face = face

    def live_view(self, vw, vh):
        """Camera frame at view size with the role's effect drawn on the tracked face."""
        img = ImageOps.contain(self.frame, (vw, vh), Image.BILINEAR)
        dw, dh = img.size
        if self.live_face:
            f = self.live_face
            fx, fy, fw, fh = f[0] * dw, f[1] * dh, f[2] * dw, f[3] * dh
            eyes = [(f[i] * dw, f[i + 1] * dh, f[i + 2] * dw) for i in (4, 7)]
            if self.step == "winner":
                paste_crown(img, fx, fy, fw, fh)
            else:
                img = draw_tears(img, eyes, dh / 720, s=1)
        else:
            self.caption(img, "Face the camera", dh * 0.9)
        return img

    def caption(self, img, text, y):
        d = ImageDraw.Draw(img)
        f = font(max(18, int(img.height * 0.045)), "SemiBold")
        d.text((img.width / 2 + 2, y + 2), text, font=f, fill=(0, 0, 0), anchor="mm")
        d.text((img.width / 2, y), text, font=f, fill=(255, 255, 255), anchor="mm")

    def role_banner(self, img):
        """Big WINNER / LOSER banner with the typed name across the top of the view."""
        title, _, color = ROLE_STYLE[self.step]
        name = self.name_var.get().strip()
        text = f"{title}  ·  {name}" if name else title
        size = max(22, int(img.height * 0.065))
        f = font(size, "Bold")
        tw = f.getlength(text)
        pw, ph = int(tw + size * 1.6), int(size * 1.6)
        x, y = (img.width - pw) // 2, int(img.height * 0.03)
        layer = Image.new("RGBA", img.size, (0, 0, 0, 0))
        d = ImageDraw.Draw(layer)
        d.rounded_rectangle((x, y, x + pw, y + ph), radius=ph // 2, fill=color + (235,), outline=(255, 255, 255, 230), width=3)
        d.text((img.width / 2, y + ph / 2 + 1), text, font=f, fill=(20, 10, 0) if self.step == "winner" else (2, 18, 44), anchor="mm")
        out = img.convert("RGBA")
        out.alpha_composite(layer)
        return out.convert("RGB")

    def countdown_overlay(self, img, n):
        layer = Image.new("RGBA", img.size, (0, 0, 0, 0))
        d = ImageDraw.Draw(layer)
        # In the corner, so it never covers the face people are lining up.
        r = int(img.height * 0.14)
        cx, cy = img.width - r - int(img.height * 0.04), img.height - r - int(img.height * 0.04)
        d.ellipse((cx - r, cy - r, cx + r, cy + r), fill=(0, 0, 0, 150), outline=(255, 255, 255, 220), width=max(2, r // 18))
        d.text((cx, cy + r * 0.04), n, font=font(int(r * 1.4), "Bold"), fill=(255, 255, 255, 255), anchor="mm")
        out = img.convert("RGBA")
        out.alpha_composite(layer)
        return out.convert("RGB")

    def show(self, img):
        self._photo_ref = ImageTk.PhotoImage(img)
        self.view.config(image=self._photo_ref)

    def tick(self):
        if self.cap.isOpened():
            ok, bgr = self.cap.read()
            if ok:
                self.frame = Image.fromarray(cv2.cvtColor(cv2.flip(bgr, 1), cv2.COLOR_BGR2RGB))
        vw, vh = max(self.view_box.winfo_width(), 100), max(self.view_box.winfo_height(), 100)

        if self.step == "card":
            key = (id(self.card), vw, vh)
            if key != self.card_view_key:   # only re-scale when the window or card changes
                self.card_view_key = key
                self.show(ImageOps.contain(self.card, (vw, vh), Image.LANCZOS))
        else:
            if self.step in self.players:
                img = ImageOps.contain(self.previews[self.step], (vw, vh), Image.BILINEAR)
            elif self.frame is not None:
                self.track_face()
                img = self.live_view(vw, vh)
                if self.countdown_end:
                    left = self.countdown_end - time.time()
                    if left <= 0:
                        self.countdown_end = None
                        self.capture()
                        img = ImageOps.contain(self.previews[self.step], (vw, vh), Image.BILINEAR)
                    else:
                        img = self.countdown_overlay(img, str(math.ceil(left)))
            else:
                img = Image.new("RGB", (min(vw, 960), min(vh, 540)), (0, 0, 0))
                self.caption(img, "No camera", img.height / 2)
            img = self.role_banner(img)
            if time.time() < self.flash_until:
                img = Image.blend(img, Image.new("RGB", img.size, (255, 255, 255)), 0.7)
            self.card_view_key = None
            self.show(img)
        self.root.after(33, self.tick)

    def quit(self):
        if self.cap.isOpened():
            self.cap.release()
        self.root.destroy()


def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--camera", type=int, default=0, help="webcam index (default 0, the built-in webcam)")
    ap.add_argument("--render", nargs=2, metavar=("WINNER_IMG", "LOSER_IMG"), help="make a postcard from two image files and exit")
    ap.add_argument("--names", nargs=2, default=("Winner", "Loser"), metavar=("WINNER", "LOSER"))
    args = ap.parse_args()

    if args.render:
        w = prepare_player(Image.open(args.render[0]), "winner")
        l = prepare_player(Image.open(args.render[1]), "loser")
        print("faces found:", w["face"] is not None, l["face"] is not None)
        print(save_postcard(make_postcard(*args.names, w, l), *args.names))
        return

    root = tk.Tk()
    BoothApp(root, args.camera)
    root.mainloop()


if __name__ == "__main__":
    main()
