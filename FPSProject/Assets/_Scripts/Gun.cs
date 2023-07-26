using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData _gunData;
    private float _timeSinceLastShot;

    private ObjectPool<TrailRenderer> _trailPool;

    public TrailConfigScriptableObject _trailConfig;
    public ParticleSystem _shootSystem;

    public CameraRecoil _cameraRecoil;

    public GunRecoil _gunRecoil;

    private void Start()
    {
        PlayerShoot.OnShoot += Shoot;
        _trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        TryGetComponent(out _gunRecoil);
    }

    private bool CanShoot() => _timeSinceLastShot > _gunData._fireRate;
    private void Shoot()
    {
        if(CanShoot())
        {
            _shootSystem.Play();
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, _gunData._maxDistance))
            {
                Debug.Log(hit.transform.name);

                StartCoroutine(
                    PlayTrail(
                        _shootSystem.transform.position,
                        hit.point,
                        hit
                        )
                    );
            }
            else
            {
                StartCoroutine(
                    PlayTrail(
                        _shootSystem.transform.position,
                        Camera.main.transform.forward * _trailConfig._missDistance,
                        new RaycastHit()
                        )
                    );
            }
            _timeSinceLastShot = 0;
            _cameraRecoil.RecoilFire();
            _gunRecoil.RecoilWeapon();
        }
    }

    /// <summary>
    /// Simulacion del disparo
    /// </summary>
    /// <param name="p_startPos">Posicion inicial (Sistema de particulas)</param>
    /// <param name="p_endPos">Posicion final de impacto o fallo</param>
    /// <param name="p_hit">Punto de impacto (new RaycastHit() si no existe)</param>
    /// <returns></returns>
    private IEnumerator PlayTrail(Vector3 p_startPos, Vector3 p_endPos, RaycastHit p_hit)
    {
        // Instanciamos el trail de la pool
        TrailRenderer instance = _trailPool.Get();
        instance.gameObject.SetActive(true); // Lo activamos
        instance.transform.position = p_startPos; // Ponemos la posicion inicial
        yield return null; // Hacemos que espere un poco para que no queden residuos del frame anterior.

        instance.emitting = true;
        float distance = Vector3.Distance(p_startPos, p_endPos);
        float distanceLeft = distance;

        while (distanceLeft > 0)
        {
            instance.transform.position = Vector3.Lerp(
                p_startPos,
                p_endPos,
                Mathf.Clamp01(1 - (distanceLeft / distance))
                );
            distanceLeft -= _trailConfig._simulationSpeed * Time.deltaTime;

            yield return null;
        }

        // Impacto de bala
        if (p_hit.collider != null)
        {
            //ManageImpact(p_hit);
        }

        instance.transform.position = p_endPos;
        yield return new WaitForSeconds(_trailConfig._duration);
        instance.emitting = false;
        instance.gameObject.SetActive(false);

        _trailPool.Release(instance); // Desaparece la bala :(
    }

    /// <summary>
    /// Crea un rastro de bala desde el pool de objetos de unity
    /// </summary>
    /// <returns>TrailRenderer para el disparo de bala</returns>
    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();

        trail.colorGradient = _trailConfig._color;
        trail.material = _trailConfig._material;
        trail.widthCurve = _trailConfig._widthCurve;
        trail.time = _trailConfig._duration;
        trail.minVertexDistance = _trailConfig._minVertexDistance;
        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        return trail;
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
    }


}
