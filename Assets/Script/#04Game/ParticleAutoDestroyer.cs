using UnityEngine;

public class ParticleAutoDestroyer : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        //����� �Ϸ�Ǹ� ����
        if(!particle.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
