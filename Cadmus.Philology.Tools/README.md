# Cadmus.Philology.Tools

This project contains various tools for working with philological data in Cadmus.

## Edit Operations

Components representing simple string editing operations, used by misspelling and potentially other Cadmus modules. The purpose of such operations is to represent changes in a simple text (usually a single word form) in a structured way. For instance, in annotating an epigraphic Latin text with a word like `bixit` instead of `vixit` (hypercorrection due to spirantization of the bilabial plosive B), one could represent the details of the correspondance between these two forms with a single replacement operation, i.e. replace `b` with `v` at position 0 to get the corresponding normalized form. This is more structured and informative than a simple string diff, and can be used for various purposes, e.g. to generate statistics on the types of errors made by scribes.

Additionally, the system also provides logic for generating the edit operations needed to transform one string into another. This is useful for automatically generating the edit operations needed to correct a misspelled word.

Edit operations are defined in a simple DSL and include both basic operations like insert and delete and higher level operations like replace, move, and swap.
