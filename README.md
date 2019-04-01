# SkillZ 2019 - Nahalal 1

This is our code (Nahalal 1)!

## Choosing combinations of heuristics

Our bot is heuristic based, meaning we defined a lot of heuristics, each of which returns a number multiplied by it's weight. In order to decide which heuristic we execute, we loop through all possible actions to check each combination of heuristics and choose the best combination.

For example, elf #1 building a portal might not be a good idea, unless elf #2 somewhere else does a thing. This way, we can know that if elf #2 does that thing, #1 should build a portal.

## Maintaining competitiveness in SkillZ

In order to maintain competitiveness for future competitions, and considering how general our bot's infrastructure is, I redacted/removed two core files ([DoNextTurnHeuristics.cs](https://github.com/ZeroByter/SkillZ-2019/blob/master/Heuristics/DoNextTurnHeuristics.cs) and [GameNextTurnActions.cs]([https://github.com/ZeroByter/SkillZ-2019/blob/master/Heuristics/GameNextTurnActions.cs](https://github.com/ZeroByter/SkillZ-2019/blob/master/Heuristics/GameNextTurnActions.cs))) which deal with selecting the best heuristics. I felt that these two files were too general and could be used too well for any sort of competition to be shared.
Every other file is specific to 2019's competition, so I left those files.

## Optimizing

We also found that trying to get an array from `ElfKingdom.Game` such as `game.GetMyLivingElves()` would result in creating a brand new array at every function call, which severely slowed down our bot's execution time and caused us to commonly timeout.

We solved this issue by creating [GameCaching.cs]([https://github.com/ZeroByter/SkillZ-2019/blob/master/Caching/GameCaching.cs](https://github.com/ZeroByter/SkillZ-2019/blob/master/Caching/GameCaching.cs)) which at the start of each turn, calls `game.GetMyLivingElves()`, but for every call after that in the same turn, it returns that same stored array.

I don't understand why they couldn't have simply done this in their own API, but that's the way it is. :/