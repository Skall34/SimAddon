# SimAddon

SimAddon is a plugin-based Windows application designed to improve the user experience for both X-Plane and Microsoft Flight Simulator. Each feature is built as an independent plugin so you can enable only what you need, extend functionality, or add future integrations.

## Table of contents
- Introduction
- Plugins
  - Flight Recorder
  - Flight Plan
  - METAR
  - ATIS
  - Chartfox
  - ATC
  - Comm
- Prerequisites
- Getting started
- Contributing
- License

## Introduction
SimAddon runs on Windows as a host application that loads lightweight plugins. It aims to provide practical tools and user-friendly interfaces to assist both virtual pilots and developers using X-Plane and Microsoft Flight Simulator. Plugins can record flight activity, manage flight plans, present weather and charts, assist with communications, and provide ATC-like aids.

## Flight Recorder
The Flight Recorder plugin records flight telemetry, control inputs, and event markers to allow later review and analysis. Typical features:
- Record and play back flights.
- Export and import recordings in common formats (e.g., GPX / CSV).
- Visual timeline with event tagging for post-flight debrief.

## Flight Plan
The Flight Plan plugin helps create, modify and share flight plans:
- Build multi-leg routes with waypoints, airways and procedures.
- Import/export formats supported by most simulators.
- Sync or push flight plans directly to the simulator when supported.

## METAR
The METAR plugin fetches and parses real-world METAR and TAF data:
- Lookup by ICAO code or nearby airports.
- Auto-refresh and configurable update intervals.
- Plain-language weather display and parsed values for easy reading.

## ATIS
The ATIS plugin generates and manages automated terminal information service messages:
- Build custom ATIS messages using live METAR/TAF data and local settings.
- Save and recall ATIS broadcasts for different airports.
- Adjustable message frequency and format.

## Chartfox
Chartfox is a chart and plate viewer plugin:
- Search and display airport diagrams, approach charts and plates.
- Offline caching for charts and imagery.
- Zoom, annotations and quick runway/procedure lookup.

## ATC
The ATC plugin provides tools to assist with traffic and clearance management:
- Display clearance and taxi instructions in a concise UI.
- Track nearby traffic and friendly aircraft.
- Phrase suggestions and logs for interactions.

## Comm
The Comm plugin centralizes radio and frequency management:
- Manage radio presets, scanning and priority frequencies.
- Auto-tune to frequencies required by other plugins (ATIS, ATC).
- Tools for quick switching between COM/NAV and intercom-like utilities.

## Prerequisites
SimAddon integrates with each simulator via third‑party bridge plugins. Before using SimAddon, ensure the appropriate bridge is installed:
- Microsoft Flight Simulator: FSUIPC (install the FSUIPC plugin required for your MSFS version).
- X-Plane: XPUIPC (the X-Plane bridge plugin required for integration).

## Getting started
1. Download and install SimAddon on Windows.
2. Start the host application and enable the plugins you need.
3. Configure simulator connection settings (X-Plane or Microsoft Flight Simulator).
4. Open the plugin UIs and follow quick-start prompts.

## Contributing
Contributions and bug reports are welcome. Please open an issue or submit a pull request following the repository contribution guidelines.

## License
See the LICENSE file in this repository for license details.
