using UnityEngine;

[RequireComponent(typeof(BusMover), typeof(BusCapacity),  typeof(BusExhaustEffect))]
[RequireComponent(typeof(AudioSource), typeof(BusShakeEffect))]
public class Bus : MonoBehaviour
{
   [SerializeField] private ColorSetter _colorSetter;
   [SerializeField] private float _modelForwardOffsetY;
   
   private BusMover _mover;
   private BusCapacity _capacity;
   private BusExhaustEffect _exhaustEffect;
   private AudioSource _engineAudioSource;
   private BusShakeEffect _shakeEffect;
   private BusSeats _seats;
   
   public BusMover Mover => _mover;
   public BusCapacity Capacity => _capacity;
   public BusLane Lane { get ; private set; }
   public StickmanColor Color { get; private set; }
   
   public float ModelForwardOffsetY => _modelForwardOffsetY;

   private void Awake()
   {
      _mover = GetComponent<BusMover>();
      _capacity = GetComponent<BusCapacity>();
      _exhaustEffect = GetComponent<BusExhaustEffect>();
      _engineAudioSource = GetComponent<AudioSource>();
      _shakeEffect = GetComponent<BusShakeEffect>();
      _seats = GetComponentInChildren<BusSeats>();
   }

   public void Initialize(BusRequest request, Vector3 startPosition, Vector3 targetPosition)
   {
      Color = request.Color;
      _colorSetter.SetColor(request.Color);
      _capacity.Initialize(request.Capacity);
      _mover.Initialize(startPosition, targetPosition);
   }

   public void SetLane(BusLane lane)
   {
      Lane = lane;
   }

   public void PlayExhaustEffect()
   {
      _exhaustEffect.PlayPuff();
   }

   public void PlayEngineSound()
   {
      _engineAudioSource.Play();
   }

   public void PlayWrongSelectionShake()
   {
      _shakeEffect.Shake();
   }

   public void Seat(Stickman stickman)
   {
      Transform seat = _seats.GetSeat(_capacity.SeatedCount);

      stickman.SitAt(seat);
      _capacity.TryBoard();
   }
}
