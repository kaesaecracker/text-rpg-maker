#! include dialogs/mayor.yaml

- id: guard-talk
  text: The area ahead is dangerous. If you want to leave the town you need a sword.
  choices:
  - text: Okay
  - text: I have a weapon. Why does it have to be a sword?
    goto-dialog: guard-metajoke
  - text: Show sword and leave
    goto-scene: fields
    required-items:
    - id: sword-wood
    
- id: guard-metajoke
  text: It has to be a wooden sword to be specific. That's just how the story was written. 
        You have to get the sword from the mayor, then you can go outside of the town to rescue the princess.