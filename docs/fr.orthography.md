# Orthography Fragment

Any relevant deviation from a standard orthography.

🔑 `fr.it.vedph.orthography`

- `location` (`string`)
- `standard` (`string`)
- `language` (`string` 📚 `orthography-languages`)
- `tag` (`string` 📚 `orthography-tags`)
- `note` (`string`)
- `operations` (`string[]`): operations are strings using some DSL. Older versions of this fragment used `MspOperation`. Newer versions use `EditOperation` and its derived classes. As each operation can have 0 or more tags, these usually come from a hierarchical thesaurus 📚 `orthography-op-tags`.

