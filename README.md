# Cadmus.Philology.Parts

This solution contains the Cadmus philologic parts library derived from `Cadmus.Philology.Parts` in the core Cadmus solution, in the context of the general refactoring towards a more streamlined and modular system. The library includes all the old parts with no additions or modifications. No part or fragment ID has been changed.

👀 [Cadmus Page](https://myrmex.github.io/overview/cadmus/)

- [Cadmus.Philology.Parts](#cadmusphilologyparts)
  - [History](#history)
    - [6.0.1](#601)
    - [5.0.0](#500)
    - [4.1.1](#411)
    - [4.1.0](#410)
    - [4.0.2](#402)
    - [4.0.1](#401)
    - [4.0.0](#400)
    - [3.0.0](#300)

## History

### 6.0.1

- 2023-02-02: migrated to new components factory. This is a breaking change for backend components, please see [this page](https://myrmex.github.io/overview/cadmus/dev/history/#2023-02-01---backend-infrastructure-upgrade). Anyway, in the end you just have to update your libraries and a single namespace reference. Benefits include:
  - more streamlined component instantiation.
  - more functionality in components factory, including DI.
  - dropped third party dependencies.
  - adopted standard MS technologies for DI.

### 5.0.0

- 2022-11-10: upgraded to NET 7.

### 4.1.1

- 2022-11-04: updated packages.

### 4.1.0

- 2022-11-04: updated packages (nullability enabled in Cadmus core).

### 4.0.2

- updated packages.

### 4.0.1

- updated packages.

### 4.0.0

- upgraded to NET 6.0.

### 3.0.0

- version numbers bumped to 3.0.0 (parts) and 2.0.0 (seeders).
