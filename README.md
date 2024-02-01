# Cadmus.Philology.Parts

👀 [Cadmus Page](https://myrmex.github.io/overview/cadmus/)

This solution contains the Cadmus philologic parts library.

- fragments:
  - [apparatus](docs/fr.apparatus.md)
  - [orthography](docs/fr.orthography.md)
  - [quotations](docs/fr.quotations.md)
  - [witnesses](docs/fr.witnesses.md)

## History

- 2024-02-01: documentation.

### 8.1.1

- 2024-01-31: updated packages.

### 8.1.0

- 2024-01-26: removed obsolete ligatures fragment model. For ligatures, use `EpiLigaturesLayerFragment`. The legacy fragment came from a limited project and used Manzella 1987 149-151:
  - `ligature`: generic ligature: a shared trait among several letters.
  - `inversion`: standard nexus with inversion of a letter for graphical commodity: e.g.`PR` where P is horizontally flipped so that its vertical trait can be shared with `R`.
  - `overlap`: letters parts overlap like `DO` where the right part of `D` and the left part of `O` cross each other, or `AV` where `V` is vertically flipped and overlapped to `A`.
  - `replacement`: a trait is shared with another letter replacing the other letter's trait which would be graphically unfit: e.g. `OE` where the curve of `O` also hosts the horizontal traits of `E`, replacing the `E`'s vertical trait.
  - `graft`: a letter continues the tracing of another letter, like `I` which in `CI` continues the topright terminal point of `C` somewhat like a vertically flipped `G`.
  - `inclusion`: improperly considered a nexus: a letter is smaller and placed inside another letter, like `O` in `C` and `I` in `G` of the word `CONIUGI`.
  - `connection`: letters are connected via additional traits not belonging to any letter.
  - `complex`: any complex combination of the other types.

### 8.0.2

- 2023-11-21: updated packages.

### 8.0.1

- 2023-11-18: ⚠️ Upgraded to .NET 8.

### 7.0.3

- 2023-09-04: updated packages.

### 7.0.2

- 2023-06-16: updated packages.

### 7.0.1

- 2023-06-02: updated packages.

### 7.0.0

- 2023-05-23: updated packages with breaking changes for general parts (introduced [AssertedCompositeId](https://github.com/vedph/cadmus-bricks-shell/blob/master/projects/myrmidon/cadmus-refs-asserted-ids/README.md#asserted-composite-id)).

### 6.0.2

- 2023-05-11: updated packages.

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
