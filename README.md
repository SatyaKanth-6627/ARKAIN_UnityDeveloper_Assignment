# ARKAIN_UnityDeveloper_Assignment

# 🎰 Unity Slot Machine Game System

## 🧩 Overview
This project is a **modular slot machine simulation** built in Unity using C#.  
It demonstrates **object-oriented game architecture**, separating UI logic, event handling, gameplay mechanics, and reel animation into distinct scripts for clean maintenance and scalability.

The system replicates a **5×4 slot grid** with horizontal match detection, credit tracking, dynamic betting, and reward visualization — designed for reusability across reel or slot-based games.

---

## ⚙️ Features

| Feature | Description |
|----------|-------------|
| 🎮 **5×4 Reel System** | Implements a 4-row, 5-column spinning slot layout. |
| 🧠 **Modular Codebase** | Independent controllers for Game, UI, Event, and Reel systems. |
| 💰 **Bet & Credit System** | Adjustable bet values with automatic credit deduction. |
| 🌀 **Coroutine-Based Spinning** | Smooth asynchronous reel animation with timing control. |
| 🔍 **Reward Evaluation** | Horizontal matching of identical symbols (3+). |
| 🪄 **Visual Feedback** | Win/Loss text animations and pulsing symbol highlights. |
| 🧩 **Dictionary Symbol Mapping** | Each sprite name mapped to its reward value. |
| 🧰 **Customizable RTP** | Modify reward array or add weighted randomness for RTP tuning. |

---

## 🧠 Architecture Overview

### 🟩 GameController.cs
- Core manager for the game lifecycle.
- Deducts bet, triggers spins, calculates rewards, and updates UI.
- Bridges between `UIController`, `ReelController`, and `EventSystem`.

**Key Methods:**
- `StartRoll()` – Begins a new round.
- `HandleReelSpin()` – Waits for reels to stop and calculates payout.
- `AddReward()` – Updates credit and reward UI.
- `OnDropdownValueChanged()` – Syncs current bet from dropdown.

---

### 🟦 UIController.cs
- Handles all on-screen text and animation.
- Animates win/loss text using scale pulsing effects.
- Updates credit and reward displays.

**Key Methods:**
- `UpdateCreditText(float)`  
- `UpdateWinAmountText(float)`  
- `DisplayReward(float)`  
- `ScaleWinText()` / `ScaleLossText()` (coroutines)

---

### 🟥 ReelController.cs
- Manages symbol generation, animation, and result evaluation.
- Ensures unique symbols per column.
- Calculates horizontal matches and total reward.
- Highlights winning cells visually.

**Key Methods:**
- `SpinReels(float)` – Runs reel spin coroutine.
- `PlayAnimation(Image, int, int)` – Spins individual cell.
- `CalculateReward(float, out List<int>)` – Evaluates row matches.
- `PulseWinningCells(List<int>)` – Pulses winning cells visually.

---

### 🟨 EventSystem.cs
- Manages bet input and dropdown interaction.
- Allows player to increase or decrease bet levels.

**Key Methods:**
- `GetSelectedAmount()` – Fetches selected bet value.
- `IncreaseAmount()` / `DecreaseAmount()` – Navigates dropdown options.

---
