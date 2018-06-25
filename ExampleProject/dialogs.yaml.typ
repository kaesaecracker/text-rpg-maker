#! include dialogs/mayor.yaml

- id: guard-talk
  text: The area ahead is dangerous. If you want to leave the town you need a sword.
  choices:
  - text: Show sword and leave
    goto-scene: fields
    required-items:
    - id: sword-wood
