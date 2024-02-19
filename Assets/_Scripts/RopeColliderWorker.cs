using System.Collections.Generic;
using _Twisted._Scripts.ElementRelated;
using Obi;
using UnityEngine;

namespace _Scripts
{
    public class RopeColliderWorker : MonoBehaviour
    {
        private ObiSolver _solver;
        private Dictionary<ObiActor, RopeElement> _actorsToRopes;

        void Awake()
        {
            _actorsToRopes = new Dictionary<ObiActor, RopeElement>();
            _solver = GetComponent<ObiSolver>();
        }
    
        void Start()
        {
            _solver.OnParticleCollision += OnRopeCollision;
        }

        private void OnRopeCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
        {
            List<ObiSolver.ParticleInActor> tempParticles = new List<ObiSolver.ParticleInActor>();
            tempParticles.AddRange(_solver.particleToActor);
            tempParticles.RemoveAll(item => item == null);
            
            foreach (Oni.Contact contact in e.contacts)
            {
                if (contact.distance < 0.01 && contact.distance > 0)
                {
                    int particleIndex = contact.bodyA;
                    int particleIndex2 = contact.bodyB;
                    // var temp = _solver.simplices[contact.bodyA];
                    // var temp1 = _solver.simplices[contact.bodyB];

                    ObiSolver.ParticleInActor pa = tempParticles[particleIndex];
                    ObiSolver.ParticleInActor pa2 = tempParticles[particleIndex2];
                    if (pa != null && pa2 != null && pa.actor != pa2.actor)
                    {
                        ActivateRope(pa.actor);
                        ActivateRope(pa2.actor);
                    }
                }
            }
        }

        private void ActivateRope(ObiActor actor)
        {
            if (_actorsToRopes.TryGetValue(actor, out var rope))
            {
                rope.InContact();
            }
            else
            {
                if (actor.gameObject.TryGetComponent<RopeElement>(out var ropeElement))
                {
                    ropeElement.InContact();
                    _actorsToRopes.Add(actor, ropeElement);
                }
            }
        }
    }
}
