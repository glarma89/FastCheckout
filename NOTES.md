\# FastCheckout Interview Task Notes



\## Initial reading plan



Before coding, I reviewed the existing application flow and focused on the files most relevant to the requested behavior:



1\. `RFIDController.cs`

&#x20;  - Main WinForms form.

&#x20;  - Owns the reader controller lifecycle.

&#x20;  - Handles UI updates, tray icon behavior, and shutdown flow.

&#x20;  - Contains the `ConfigureHotkey()` placeholder.



2\. `GlobalKeyboardHook.cs`

&#x20;  - Already scaffolded with WinAPI signatures and fields.

&#x20;  - Intended location for the global keyboard hook implementation.



3\. `Configuration/config\_computer.json`

&#x20;  - Reviewed as a possible place for a configurable hotkey follow-up.



4\. `RfidEpcParser.cs`

&#x20;  - Reviewed as a possible follow-up for decoded barcode display.



\## Implemented behavior



Pressing `S` anywhere in Windows toggles RFID inventory:



\- First press starts inventory.

\- Second press stops inventory.

\- The key is not swallowed.

\- Typing `s` into another application still works normally.



\## Technical approach



I implemented the existing `GlobalKeyboardHook` scaffold using a low-level Windows keyboard hook:



\- `SetWindowsHookEx`

\- `WH\_KEYBOARD\_LL`

\- `CallNextHookEx`

\- `UnhookWindowsHookEx`



The hook observes key presses globally and raises the existing `KeyPressed` event.



The form subscribes to `KeyPressed` in `ConfigureHotkey()` and toggles inventory when the key is `Keys.S`.



UI-related work is marshalled back to the WinForms UI thread using `BeginInvoke`.



\## Alternatives considered



\### RegisterHotKey



Could be used for a global hotkey, but it is better suited for key combinations such as `Ctrl + Alt + S`.

For a plain `S` key, it may interfere with normal typing expectations and is less appropriate for this requirement.



\### Application.AddMessageFilter



This only works reliably for messages within the current application message loop.

It would not detect `S` when another application is focused.



\### Low-level keyboard hook



Chosen because it detects the key globally while allowing the keystroke to continue to the focused application via `CallNextHookEx`.



\## Preserved behavior checklist



\- App launches and connects on startup.

\- Reader status becomes connected.

\- Tags display newest-first with timestamps.

\- Tag count updates.

\- Tray icon behavior remains unchanged.

\- Closing the window hides it instead of exiting.

\- Tray Close exits cleanly.

\- Reader shuts down on exit.

\- Typing `s` in another app still produces `s`.



\## Possible follow-ups



1\. Move the hardcoded `S` key to `Configuration/config\_computer.json`.

2\. Show decoded barcode using `RfidEpcParser.ExtractBarcodeFromRfid`.

3\. Add per-session tag deduplication.

4\. Design auto-reconnect with retry/backoff.


## Implemented follow-up: configurable toggle key

I implemented the configurable toggle key follow-up.

The inventory toggle key is now read from:

`Configuration/config_computer.json`

```json
"Hotkeys": {
  "InventoryToggleKey": "S"
}

If the setting is missing, empty, or cannot be parsed as a valid Keys value, the application falls back to S.

I tested both:

Default S
Custom value such as F8

The hotkey remains non-blocking: the key press is observed globally but still reaches the focused application.