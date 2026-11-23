# Weight Calculater

This repository contains separate Windows and Linux versions of the weight
calculator:

- `Windows/VisualBasic/` — Visual Basic Windows Forms project for Visual Studio.
- `Linux/Gambas3/` — Gambas 3 project.

Open the solution in `Windows/VisualBasic/` with Visual Studio, or open
`Linux/Gambas3/.project` with the Gambas 3 IDE.

## Development and releases

Develop each project in its platform folder. When a platform version is ready,
tag its release independently:

- Windows: `windows-vMAJOR.MINOR.PATCH` (for example, `windows-v1.0.0`)
- Linux: `linux-vMAJOR.MINOR.PATCH` (for example, `linux-v1.0.0`)

Attach the corresponding platform's build output to its release. Keep generated
build files out of the source folders.
