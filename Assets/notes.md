## Networking tips
* [Disable server-only components on client](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/v0.2.1/Assets/BossRoom/Scripts/Server/Game/Entity/ServerEnemyPortal.cs#L48)
  * Will `GetComponent<>()` in `Awake` still find them?
* [Use `Initialize` for "constructor"](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/v0.2.1/Assets/BossRoom/Scripts/Server/Game/Entity/ServerProjectileLogic.cs#L63)
  * Any network stuff needs to be done in NetworkStart
* [NetworkSpawnManager.SpawnedObjects.TryGetValue](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/v0.2.1/Assets/BossRoom/Scripts/Server/Game/Entity/ServerProjectileLogic.cs#L152)
 
## General Unity tips
* [Tooltip](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/v0.2.1/Assets/BossRoom/Scripts/Server/Game/Entity/ServerEnemyPortal.cs#L26)
* [Enums in interface](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/3db926d88c0518287630bf29e5a0c28540413709/Assets/BossRoom/Scripts/Server/Game/Entity/IDamageable.cs#L30)
* [Get components in Awake](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/v0.2.1/Assets/BossRoom/Scripts/Server/Game/Entity/ServerEnemyPortal.cs#L42)
* [Start coroutine's name with `Coro...`](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/v0.2.1/Assets/BossRoom/Scripts/Server/Game/Entity/ServerEnemyPortal.cs#L100)
* [Throw ArgumentNullException](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/3db926d88c0518287630bf29e5a0c28540413709/Assets/BossRoom/Scripts/Server/Game/Entity/ServerWaveSpawner.cs#L223)
  *  if editor-assigned property is null
* [Debug.Assert](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/3db926d88c0518287630bf29e5a0c28540413709/Assets/BossRoom/Scripts/Server/Game/State/ServerBossRoomState.cs#L165)


## NetworkStart
1. Disable component on client (`if (!IsServer) { enabled = false; }`)
1. `m_IsStarted = true`
1. Create collision masks
1. Save `transform.position` etc. in networked variable

## Unit spawning
1. do it at the end of NetworkStart if spawning is automatic
1. `clone = Instantiate()`
1. set network variables and private properties (use Initialize)
1. `if(!clone.isSpawned) clone.Spawn(null, true);`

## Game Logic
1. Everything is started in `NetworkStart` (this method has similar role as `Start`)
1. All logic is run on server, component is disabled on client with `if (!IsServer) { enabled = false; }`
1. [Example](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop/blob/3db926d88c0518287630bf29e5a0c28540413709/Assets/BossRoom/Scripts/Server/Game/Character/ServerCharacter.cs)