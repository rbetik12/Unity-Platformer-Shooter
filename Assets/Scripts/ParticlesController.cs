using UnityEngine;
using UnityEngine.Tilemaps;
public class ParticlesController : MonoBehaviour {

    private enum BlockType {
        GLASS,
        STONE,
        BOMB
    }

    [SerializeField] private ParticleSystem playerDeathParticles;
    [SerializeField] private ParticleSystem enemyDeathParticles;
    [SerializeField] private ParticleSystem playerCollisionParticles;
    [SerializeField] private ParticleSystem bulletDestroyPatricles;
    [SerializeField] private ParticleSystem blockDestroyParticles;

    public void PlayerDeathParticles(Vector3 deathPosition) {
        playerDeathParticles.transform.position = deathPosition;
        playerDeathParticles.Play();
    }

    public void EnemyDeathParticles(Vector3 deathPosition, Color particlesColor) {
        enemyDeathParticles.transform.position = deathPosition;
        ParticleSystem.MainModule ma = enemyDeathParticles.main;
        ma.startColor = particlesColor;
        enemyDeathParticles.Play();
    }

    public void PlayerCollidePatricles(Vector3 pos, Color color) {
        pos = new Vector3(pos.x, pos.y + 0.2f, pos.z);
        ParticleSystem.MainModule ma = playerCollisionParticles.main;
        ma.startColor = new ParticleSystem.MinMaxGradient(color);
        playerCollisionParticles.transform.position = pos;
        playerCollisionParticles.Play();
    }
    public void BulletDestroyPatricles(Vector3 pos) {
        bulletDestroyPatricles.transform.position = pos;
        bulletDestroyPatricles.Play();
    }

    public void OnBlockDestroyParticles(Vector3 pos, Color color) {
        ParticleSystem.MainModule ma = blockDestroyParticles.main;
        ma.startColor = new ParticleSystem.MinMaxGradient(color);
        blockDestroyParticles.transform.position = pos;
        blockDestroyParticles.Play();
    }

    public void OnBlockDestroyParticles(Vector3 pos, Tile tile) {
        if (tile == null) return;
        ParticleSystem.MainModule ma = blockDestroyParticles.main;
        ma.startColor = new ParticleSystem.MinMaxGradient(GetParticlesColor(tile));
        blockDestroyParticles.transform.position = pos;
        blockDestroyParticles.Play();
        Debug.Log(GetParticlesColor(tile));
        Debug.Log(pos);
    }

    private Color GetParticlesColor(Tile tile) {
        BlockType type = GetBlockTypeByName(tile);
        switch (type) {
            case BlockType.BOMB:
                return new Color(1f, 0f, 0f);
            case BlockType.GLASS:
                return new Color(153 / 256f, 217 / 256f, 234 / 256f);
            case BlockType.STONE:
                return new Color(74 / 256f, 74 / 256f, 74 / 256f);
            default:
                return Color.white;
        }
    }

    private BlockType GetBlockTypeByName(Tile tile) {
        string blockName = tile.name;
        switch (blockName) {
            case "Glass":
                return BlockType.GLASS;
            case "stone_block":
                return BlockType.STONE;
            case "Bomb":
                return BlockType.BOMB;
            default:
                return BlockType.BOMB;
        }
    }
}