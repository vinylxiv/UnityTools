(v3.0.0)
--------
- [CRITICAL] Implemented new licensing! Stabler. Much better response time. Less down time. Manual license transfer, bug reporting and feedback features!
- [Improvement] Failed patches won't make other patches fail too and can now retry.
- [Fix?] A patch may fail due to special characters in the project name. Retrying patching should fix it.

(v2.9.4)
--------
- [CRITICAL] Fixed an issue that causes CE to lockup when going into playmode with domain reload.
- [Feature] Added 'Convert' to animator parameters for all types. Note: Converted parameters may not handle conditions the same way they did with their original type.
- [Feature] Added 'Sample from Active StateMachine' to Layer node positions.
- [Feature] Added 'Reverse Adjusts Values' to settings. This will modify the value/threshold of conditions with less/greater when reversing to properly reflect the opposite value.
- [Fix] Parameter rename window will now appear in the middle of your animator window
- [Fix] Fixed restoring default settings for 'Default State' setting name as blank.
- [Misc] Moved 'Legacy Dropdown' to window's context menu

(v2.9.2)
--------
- [CRITICAL] Fixed an inconsistent bug that sometimes causes the HWID to be reported incorrectly or the authentication dropping randomly.
- [Improvement] Massively improved the speed of renaming behaviour parameters when renaming through the parameters list.
- [Fix] Transitions from state to itself will now always have 'Can Transition To Self' On regardless of default settings.
- [UI] Clips with loop time On now have a loop icon on states.
- [Misc] Update checker doesn't require you to open the settings anymore

(v2.9.1)
--------
- [Feature] Added ability to choose whether Quick Toggle merges with existing clips or replaces them.
- [Fix] Fixed Quick Toggle window appearing tiny in the top left corner of the screen
- [Fix] Fixed layer's default node positions not changing and saving

(v2.9.0)
--------
- [Feature] Implemented CE QuickToggle! Drag and Drop a gameobject to a state to use.
- [Feature] Implemented Template dynamic renaming!
- [Feature] Right click a condition's parameter to change it using keyboard. Write a non-existent parameter to add it quickly.
- [Feature] Implemented custom condition matchin options. Click the cog icon on conditions to activate temporarily.
- [Feature] Ability to set default Entry, Exit and AnyState positions for newly added layers.
- [Feature] Added 'Add Root Tree' to Blendtree nodes.
- [Improvement] If multiple states are selected, drag n drop of motion onto one of those states will set it for all of them
- [Fix] Fixed new empty states not using custom default motion.
- [Fix] Fixed inability to undo condition changes.
- [Fix] Fixed Quick Clip ('+') opening your documents as path by default due to previously saving a clip in a location that doesn't exist in the current project.
- [Fix] Fixed the ability to brick your controller by replicating an Exit transition from Entry.
- [Fix] Fixed Replace Parameter only affecting the second parameter of 'Copy' parameter drivers.
- [Fix] Fix clearing settings not working properly if settings window wasn't opened beforehand.
- [UI] Fixed the 'Add' button for missing parameters going outside the alloted GUI area
- [UI] Fixed minor GUI issue with Trigger conditions
- [Patch] Renaming parameter will also reflect in parameter drivers
- [Patch] Adding a parameter will now make the scrollbar go down to the bottom.
- [Patch] Visual indicator to differentiate between float and integer parameters easier.
- [Misc] Slight changes to saving and settings UI. Made need to set some settings again.
- [Misc] States with (WD on/off) in their name will now be ignored when mass setting write defaults

(v2.8.4)
--------
- [Fix] Fixed Transitions to StateMachine not using custom default settings
- [Fix] Fixed blend tree parameters not copying when using the controller tab copy function
- [Fix] States alignment using 'Align Horizontal' and 'Align Vertical' can now undo
- [Fix] Automatically refreshes graph when making multiple transitions from Entry
- [Misc] Inverted 'Alternate Double Click''s Behaviour so that Off would be using Unity's native Double Click behaviour. Sorry for any confusion!
- [Misc] Added 'Instructions' to CEditor's context menu and a help button next to settings button to open CEditor's manual.

(v2.8.3)
--------
- [Fix] Fixed inability to access SubStateMachines
- [Misc] Replaced funky "X" button on lists with the usual reorderable list's '-' button
- [Misc] Alternate Double Click is now on by default for first use

(v2.8.2)
--------
- [Feature] Added Copy, Paste and Remove for behaviours on States and Statemachines
- [Fix] If using Ctrl for chain connecting, you now can still connect to and start connecting from target state with double click
- [Fix] Fixed redirect transition not redirecting to Exit if only Exit is selected.
- [Fix] Fixed Any and Entry requiring Ctrl to start chain connecting

(v2.8.1)
--------
- [Feature/Misc] Added "Fake Transition*". Read note below.
- [Fix] Fixed pressing enter toggles 'Make Multiple Transitions'
- [Fix] Removed debug from dev testing
- [Misc] New empty states will now be named the same as the default state in settings.
- [Misc] Re-enabled state name editing for selected states settings.
- [Misc] Re-enabled state name editing for default state's settings.
- [Misc] Changed main tabs toggle functionality. Ctrl/Shift click to enable multiple.

Fake Transition: Do Ctrl+Shift+Click (x2) on any Node to do a fake transition.
You can make a transition from ANY node to ANY other node including itself. These transitions are only for fun. They aren't functional and will not be saved.
They may throw an error rarely but should be relatively safe.