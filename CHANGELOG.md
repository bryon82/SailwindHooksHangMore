# Changelog

All notable changes to this project will be documented in this file.

## [v2.1.2] - 2026-07-08

### Fixed
- Bug where if a hangable item and a hook are in inventory slots they "connect" when game is loading.

## [v2.1.1] - 2026-06-28

### Fixed
- Bug caused by eating or selling an item and then looking at a hook.
- Buckets falling from hooks when loading a save where they were hanging.
- Ships added via mod not loading properly when they had anchors attached to a hook.

## [v2.1.0] - 2026-06-26

### Added
- The ability to attach Anchors.

## [v2.0.0] - 2026-06-21

### Changed
- Some items now hang dynamically instead of being statically attached. Items that hang dynamically: kettles, buckets, fish, mugs, and pots. Items that are still statically attached: fishing poles, knives, oars, chip logs, quadrants, brooms, and hammers.

### Updated
- Guid, removed the 82.

## [v1.0.9] - 2026-06-08

### Added
- The ability to hang pots, big pots, wooden mugs, and metal mugs.

### Updated
- Buckets now have to be emptied to hang.

## [v1.0.8] - 2026-06-06

### Updated
- Changed the release process so hopefully some linux users will no longer have issues when unzipping the release file.

### Added
- The ability to hang oars.

## [v1.0.7] - 2026-02-23

### Added
- The ability to hang hammers.

## [v1.0.6] - 2025-09-15

### Added
- The ability to hang buckets.
- The ability to hang kettles.

## [v1.0.5] - 2025-08-28

### Added
- The ability to hang fish. Fish will dry while hung on a lamp hook.

## [v1.0.4] - 2025-08-12

### Fixed
- Timing conflict with NANDFixes where both mods patch the lamp hook. This made the lamp hook unusable.

## [v1.0.3] - 2025-08-09

### Added
- For those concerned with safety, config entry to flip the knife rotation in the lamp hook.

### Fixed
- NRE on game startup related to the previous quadrant update.
- NRE when looking at a lamp hook after eating food.

## [v1.0.2] - 2025-08-08

### Fixed
- Quadrant plumb line flailing around when quadrant is attached to hook.

## [v1.0.1] - 2025-08-07

### Fixed
- An error thrown when shop item ItemSpawner spawns a new attachable item.

## [v1.0.0] - 2025-08-05

### Added
- Moved over lamp hooks being able to hold a fishing rod, chip log, broom, quadrant, and knives from BetterFishing.
