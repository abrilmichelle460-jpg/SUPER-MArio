# Underground and underwater stages

## Scenes and progression

- `Assets/Scenes/1-2.unity`: underground stage based on the main World 1-2 map and its coin room.
- `Assets/Scenes/1-3.unity`: underwater stage based on the World 2-2 map.
- Build order: `1-1` → `1-2` → `1-3`. The original `1-1.unity` file and all original prefab assets remain unchanged.
- Default flag destinations now advance to the next stage. The new flags explicitly target `1-3` and `1-4` respectively.
- Opening either new scene directly starts at that stage instead of reloading `1-1`.
- The missing `1-4` is handled as campaign completion: a coming-soon panel appears with Enter to restart. Adding `Assets/Scenes/1-4.unity` to Build Settings activates the configured destination.

## Controls and gameplay

- A/D or Left/Right: movement. Space: jump; press repeatedly to swim upward in water.
- S: enter the underground bonus pipe. D: enter horizontal return/exit pipes.
- Big Mario breaks ordinary bricks from below; small Mario bumps them. Question/item blocks keep their item behavior. Hard blocks remain solid.
- The underground stage contains 33 free coins, five item blocks, ordinary brick cells, eight Goombas, one Koopa, two vertical elevators, a coin room and a surface flag/castle exit.
- The aquatic stage contains 28 free coins, seven Bloopers, eight Cheep Cheeps, coral, underwater rock formations and a surface flag/castle exit.
- Swimming is opt-in through the aquatic stage settings. Ground movement parameters and the original Mario prefab are retained. Water slows horizontal movement and falling, adds repeatable swim impulses, and limits ascent at the surface. Ground movement returns on leaving the sea.

## Reused resources

The scenes retain instances of the original Mario, ground, underground rock, bricks, hard blocks, question blocks, coins, pipes, pipe connections, Goomba, Koopa, clouds, hill, flagpole and castle prefabs. Sprite overrides are scene-local. The bonus room uses the existing pipe coroutine and camera height switching. Static terrain spans use tiled renderers and box colliders; breakable bricks are individual instances.

## New resources

Two reusable enemy prefabs: `Assets/Prefabs/Levels/Blooper.prefab` and `CheepCheep.prefab`.

Ten small PNG assets under `Assets/Sprites/Levels/`, imported as Single sprites with 16 PPU, Point filtering, Clamp wrapping, no mipmaps and no compression:

| Asset | Size | Source and purpose |
| --- | --- | --- |
| UndergroundHardBlock | 16×16 | Tileset sheet rectangle (147,33)–(163,49); underground hard block |
| UndergroundGoomba1 / 2 | 16×16 each | Enemy sheet rectangles (74,16)–(90,32) and (92,16)–(108,32); blue underground palette |
| UnderwaterGround | 16×16 | Tileset rectangle (147,134)–(163,150); sea-floor rock |
| Coral | 16×48 | Tileset rectangle (312,640)–(328,688); decorative coral, without collision |
| Blooper1 / 2 | 16×24 each | Enemy sheet rectangles (224,52)–(240,76) and (242,52)–(258,76) |
| CheepCheep | 16×16 | Enemy sheet rectangle (276,52)–(292,68) |
| WaterSurface | 16×8 | Repeating wave tile from World 2-2 reference rectangle (0,272)–(16,280); no collision |
| Pixel | 1×1 | White utility pixel for the sky-color rectangle |

Sheet background colors were made transparent around enemy/coral cutouts. No reference ZIP was imported and no complete map image is used as gameplay scenery. Map images only supplied positions for modular geometry and coins.

## Scripts

- `BlockHit.cs`: detection of hits from below, big-Mario brick destruction, four temporary fragments, and preserved item-block behavior.
- `GameManager.cs`: direct scene initialization, progression and missing-stage handling.
- `FlagPole.cs`: one completion sequence per flag, default next-stage progression.
- `Pipe.cs`: prevents overlapping entry coroutines.
- `SideScrolling.cs`: optional right-hand camera boundary.
- `PlayerMovement.cs`: opt-in swimming, with ground behavior preserved.
- `Levels/LevelRuntime.cs`: stage HUD, environment colors, water/land switching and campaign-complete panel.
- `Levels/MovingPlatform.cs`: vertical kinematic elevators.
- `Levels/AquaticEnemy.cs`: aquatic patrol movement, sprite frames and damage/star-power interaction.

## Approximations and remaining work

The terrain and coin positions follow the reference grid. Enemy placement and patrols are approximations because the supplied level maps omit enemies. World 1-2 starts directly in the cave; its exterior entrance and warp-zone destinations are omitted. Two elevators approximate the original lift section. The exit staircases, background scenery, some extra interactive bricks and pipe connections are adapted to existing prefabs. The underwater enemies use simple patrols rather than the original NES AI. Mario reuses existing jump/run sprites while swimming. Several sheet colors differ slightly from the level-map palette.

Play testing is performed by the project owner. Compilation and asset/scene references are checked during implementation; a complete manual playthrough and difficulty tuning remain with the owner.

- Castle: build the future `1-4` scene, add Bowser/hazards and include it in Build Settings.
- Game over: replace the existing restart behavior with a dedicated screen and retry flow.
- Audio: add licensed/approved music, AudioSources and gameplay sound hooks. The supplied audio ZIP remains reference material only; no audio clips were imported.
