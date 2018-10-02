# TextRpgCreator
An engine for running your self-created text adventures! If you want to create a game, this program helps you avoid writing any code at all. As a player, you get a nice UI and do not have to type as much.

Currently only Linux is officially supported. It is not tested on macOS, but should work. On Windows, some work is probably required to get .NET Core to load the WPF dlls and so on.

## Project structure
A game/project is basically a folder containing specific YAML files. You should start by looking at the example project.
You can generate a type documentation from within the application under Help->Creators->Export yaml type documentation.

- project-info.yaml: meta data, start characters etc
- characters.yaml
- dialogs.yaml
- 
- items/
  - weapons.yaml
  - armor.yaml
  - comsumables.yaml
  - ammo.yaml
 
## TYP files
You might want to split your YAML files into multiple ones. To do this, rename your `foo.yaml` to `foo.yaml.typ`. Now you can use TextRpgMaker YAML Preprocessor features, which at this point is to use `#! include <path>` where `<path>` is a path relative to the project root. The contents of the specified file will replace the original line. 

When loading a project, the preprocessor first generates a .yaml for every .typ. The original .typ will not be changed, but the .yaml will be overwritten.

`#! include`-ing a .typ is currently not supported.

## Already working
- Loading a project (improper formatting etc should not crash the app)
- Starting a new game
- Choosing the start character and dialog options from the dropdown
- dialogs, except
    - required and reward items
    - moving to a different scene
- specifying items etc in the files

## Icon
Icon from https://icons8.com - free commercially usable icons
