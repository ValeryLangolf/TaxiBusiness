using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _passengerShowing;
    [SerializeField] private AudioClip _passengerHiding;
    [SerializeField] private AudioClip _engin_roar;
    [SerializeField] private AudioClip _passengerGoInCar;
    [SerializeField] private AudioClip _gettingRevenue;
    [SerializeField] private AudioClip _unsuccessfulPaymentAttempt;
    [SerializeField] private AudioClip _vehicleSelected;
    [SerializeField] private AudioClip _vehiclePurchased;

    public static SfxPlayer Instance { get; private set; }

    private AudioSource _source;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _source = GetComponent<AudioSource>();
    }

    public void PlayPassengerShowing() =>
        _source.PlayOneShot(_passengerShowing);

    public void PlayPassengerHiding() =>
        _source.PlayOneShot(_passengerHiding);

    public void PlayEngineRoar() =>
        _source.PlayOneShot(_engin_roar);

    public void PlayPassengerGoInCar() =>
        _source.PlayOneShot(_passengerGoInCar);

    public void PlayGettingRevenue() =>
        _source.PlayOneShot(_gettingRevenue);

    public void PlayUnsuccessfulPaymentAttempt() =>
        _source.PlayOneShot(_unsuccessfulPaymentAttempt);

    public void PlayVehicleSelected() =>
        _source.PlayOneShot(_vehicleSelected);

    public void PlayVehiclePurchased() =>
        _source.PlayOneShot(_vehiclePurchased);
}