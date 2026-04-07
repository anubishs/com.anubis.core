# Migration Notes

## DateMananger -> DateManager

`DateMananger` has been deprecated and replaced by `DateManager`.

- New usages should add `DateManager` to GameObjects.
- Existing scenes using `DateMananger` remain source-compatible for now through an `[Obsolete]` compatibility class.
- Plan to remove `DateMananger` in a future major version.
