# Dota2 Meta Updater

Grabs top winners for each position from Stratz and turns them into a custom grid. Your existing grid will be left alone.

![Example](/Media/example_1.png?raw=true "Example")

## Grid setup
In config.json you can find a default setup, but the following changes are **required** for it to function:
* steamPath: If you do not have steam installed in it's default location, you have to change it here.
* accountId: Enter your steam account ID here. This is used so the correct folder can be selected that holds the dota2 config. You can find this by looking at the third number in your SteamID3 number (eg: [U:1:XXXXXXX]). SteamID3 can be found used any number of websites.

Each entry in gridConfig will add a new option to the dropdown list in the heroes window. Additionally, for every extra bracket you fill in (above 1), will also generate extra list items.

Other options:
* name: The name used in the dropdown list. There are some template values you can used to have some dynamic values, see below table.
* Heigh/width might need to be adjusted to your screen. 
* topWins: The number of heroes to display

There is also the option to have custom columns of fixed heroes. You can use this to have a personal pick/ban list. Every entry is a column. The name accepts no template values. The heroes are the names as you see them ingame, case insensitive.

## Grid name template options:

| Option      | Meaning                                      | Example                    | Comment                        |
|-------------|----------------------------------------------|----------------------------|--------------------------------|
| \$bracket$   | Current bracket                              | Immortal                   |                                |
| \$version$   | Current game version                         | 7.30e                      |                                |
| \$longdate$  | Long version of the current date (not time)  | Saturday, October 30, 2021 | Based on your machine settings |
| \$shortdate$ | Short version of the current date (not time) | 10/30/2021                 | Based on your machine settings |

## TODO:
* Make error catching a bit nicer
* If you only have 1 folder in your userdate, use that (no more manual entry)
* Actual winrate instead of just ordering by amount of wins.
* Auto update on a schedule
* Nicer interface (maybe)

## Known Issues
* Every so often, Stratz' service seems to be unavailable for a split second. This will throw an exception, but a rerun goes smoothly. 

I am also aware i'm terrible at writing documentation.