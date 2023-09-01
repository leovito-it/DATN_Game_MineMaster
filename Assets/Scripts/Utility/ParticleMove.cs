using System.Collections;
using UnityEngine;

public class ParticleMove : MonoBehaviour
{
    [SerializeField] private ParticleSystem system;
    [SerializeField] private Transform target;

    private readonly float particleSpeed = 5f;
    private readonly float delayMoveTime = 0.4f;

    private ParticleSystem.Particle[] particles;
    private bool showFx;

    private void OnEnable()
    {
        this.showFx = false;
    }

    private void Update()
    {
        this.StartCoroutine(this.WaitToShowFx(true));
        if (!this.showFx) return;
        if (!this) return;
        this.particles = new ParticleSystem.Particle[this.system.particleCount];
        var particleCount = this.system.GetParticles(this.particles);
        for (var i = 0; i < particleCount; i++)
        {
            var particle = this.particles[i];
            var distance = Vector3.Distance(particle.position, this.target.position);
            if (distance > 0.1f)
            {
                particle.position = Vector3.Lerp(particle.position, this.target.position, this.particleSpeed * Time.deltaTime);
            }

            this.particles[i] = particle;
        }

        this.system.SetParticles(this.particles, particleCount);
    }

    private IEnumerator WaitToShowFx(bool isShow)
    {
        yield return new WaitForSeconds(this.delayMoveTime);
        this.showFx = isShow;
    }
}
