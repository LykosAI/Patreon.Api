# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.1] - 2024-11-03
### Fixed
- Fixed IPatreonApi.GetToken missing types error for source generated json deserialization.

## [1.2.0] - 2024-10-12
### Added
- Added fields parameters for tier to IPatreonApi.GetIdentity and IPatreonApi.GetIdentityJson.

## [1.1.1] - 2024-10-10
### Fixed
- Fixed IPatreonApi.GetIdentityJson missing types error for source generated json deserialization.

## [1.1.0] - 2024-10-09
### Added
- Added IPatreonApi.GetIdentity overloads that accept field parameters.
- Added IPatreonApi.GetIdentityJson overloads that return JsonObject instead of the typed models for custom parsing.

## [1.0.0] - 2024-10-08
### Changed
- Updated documentation.

## [1.0.0-pre.2] - 2024-10-08
### Fixed
- Fixed IPatreonApi.GetIdentity missing Property attribute value error

## [1.0.0-pre.1] - 2024-10-07
### Added
- Initial release.
