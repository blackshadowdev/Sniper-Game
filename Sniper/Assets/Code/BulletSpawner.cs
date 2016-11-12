using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour {

	[SerializeField] private Transform _bulletSpawnPos;
	[SerializeField] private Transform _bulletPrefab;
	[SerializeField] private ParticleSystem _muzzleFlashPfx;
	[SerializeField] private AudioSource _gunAudioSource;
	[SerializeField] private AudioClip _gunAudioClip;

	public void Fire ()
	{
		_bulletSpawnPos.LookAt(PlayerHead.Ins.transform);														
		Instantiate(_bulletPrefab, _bulletSpawnPos.position, _bulletSpawnPos.rotation);			
		_muzzleFlashPfx.Emit(10);																
		_gunAudioSource.PlayOneShot(_gunAudioClip);												
	}
}
