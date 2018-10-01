# Multitasking
#### X-5-120 Working Interactive

**Key Message**

Research shows that there is no such thing as multitasking.

**Experience Overview**

Visitors observe an informational display and take a
multitasking simulator test.

-----

## Technology
- Created in Unity using the [RotoBase DLL](https://code.roto.com/standard-applications/unity-roto-base)
- Tween creation using [DOTween](http://dotween.demigiant.com/documentation.php)
- Some text rendering done using [TextMesh Pro](https://www.assetstore.unity3d.com/en/#!/content/84126)

-----

## Application Flow

Visitors walk up to the interactive and touch the screen to begin.  An overlay slides left, revealing and intro screen.  After reading the intro screen, it fades to reveal a set of three instructions, each one telling the visitor how one of the in-game tasks works.

The first task tests reaction skill.  Colored triangles appear in a black zone, and travel to the right.  When the triangle is in the zone that matches its color, that colored section can be touched in order to score points.  There are three colored sections that match to three colored triangles; red, yellow, and green.  The triangles will appear at varying times at varying speeds with random heights.  The variation is all specified into various timed-sections.  Each timed section contains information about the number of triangles in the timed section, the spawn rate of the triangles (in seconds), the speed of the triangles to get from one side to the other (in seconds), and the number of points the visitor receives for touching the correct section at the correct time.

The second task tests calculation and math.  At the same time visitors are attempting to score points on the reaction task, simple math problems will fade into view, allowing the visitor to score points for correctly answering the math problem.  The math problems are simple arithmetic, and are randomly chosen to be addition, subtraction, multiplication, and division.  Only single digit integer are used for values, and the answer can be multiple digits, but is always positive and an integer.  The math problem is displayed with a set of four multiple choice answers in a two-by-two grid, with one of the possible answers being the correct one.  Visitors score points and the math problem fades away if the correct answer is selected.  If an incorrect answer is selected, or the allotted time for selecting an answer is up, the math problem also fades away.

The third task appears at the same time as the reaction and calculation tasks and tests memory.  A randomly generated set of letters fades in to the middle of the screen.  The letters are all uppercase and may contain the same letter twice, but are ensured to not contain a naughty word or phrase.  After a set amount of time the letters fade.  Then, after a period of no letters on screen, four multiple choice options fade in, only one of the options having the same letters in the same order as the letters that appeared before.  Again, visitors score points and the letters fade away if the correct answer is selected.  If an incorrect answer is selected, or the allotted time for selecting an answer is up, the letters also fade away.

The game flow for the tasks is as follows:  First, the reaction task appears by itself.  Then, after a set amount of time, while the reaction task is still running, the calculation task appears.  Then, after another set amount time, while both of the other tasks are still running, the memory task appears.  Visitors will perform all three tasks for another set amount of time, until the challenge is done.  Each time points are awarded the button or triangle will pulse, but if an incorrect options is selected, the selected answer will shake.

After the tasks are completed a results screen is shown.  Visitors touch the on-screen button to reveal their score, which is a percentage of their score from the overall total possible points.  Two options are also displayed, one for starting over, which will take the visitor back to the instructions screen, and one for ending the interactive, which will go back to the attract screen.

The interactive can time out at any time due to inactivity.

-----

## Configuration Settings

The configuration values can be set in the ***config.json*** located in the ***StreamingAssets*** folder.

#### Main application
| Setting | Purpose | Type | Default |
|-------- | ------- | ---- | ------- |
| timeout-seconds | Seconds of inactivity before the application resets. | float | 40 |
| results-duration | The number of seconds after the result is displayed of inactivity before the application resets. | float | 10 |

#### Reaction Task
| Setting | Purpose | Type | Default |
|-------- | ------- | ---- | ------- |
| spawn-locations | The number of vertical locations that the triangles will be spawned from. | int | 5 |
| timed-sections | Array of timed sections that the triangles will go through in order. |

##### Timed Section example
Each timed section contains information about the number of triangles in the timed section, the spawn rate of the triangles (in seconds), the speed of the triangles to get from one side to the other (in seconds), and the number of points the visitor receives for touching the correct section at the correct time.

``` json
{"points": 1, "spawn-rate": 1.5, "num-of-triangles": 4, "speed": 4}
```

#### Calculation Task
| Setting | Purpose | Type | Default |
|-------- | ------- | ---- | ------- |
| appear-time | Seconds of game time before the calculation task appears. | float | 15 |
| points-per-question | The number of points received for the correct answer. | float | 5 |
| show-seconds | Duration of seconds that the math problem is shown before it fades away due to inactivity. | float | 5 |
| next-question-seconds | Seconds from showing one math problem to showing the next. | float | 8 |

#### Memory Task
| Setting | Purpose | Type | Default |
|-------- | ------- | ---- | ------- |
| appear-time | Seconds of game time before the memory task appears. | float | 25 |
| points-per-question | The number of points received for the correct answer. | float | 8 |
| show-question-seconds | Duration of seconds that the question letters is shown before it fades away due to inactivity. | float | 4 |
| hide-question-seconds | Seconds after showing the question letters before showing the possible answers. | float | 1 |
| show-answers-seconds | Duration of seconds that the multiple choice answers are shown before they fade away due to inactivity. | float | 5 |
| next-question-seconds | Seconds from showing one memory problem to showing the next. | float | 13 |

-----

## Application Shortcuts

### Mouse Cursor
The mouse cursor is hidden by default.  A user can press the ***\`*** (back quote) key at any time to toggle the mouse cursor visibility.


### Exiting the App
The application can be exited at any time in a few ways
- using the corner codes in the "12432" sequence
- pressing "Esc" on the keyboard


## Debugging Shortcuts
- You can skip straight to all tasks by pressing the ***Z*** key.
- You can skip to the end of the tasks, once they are started, by pressing the ***X*** key.
