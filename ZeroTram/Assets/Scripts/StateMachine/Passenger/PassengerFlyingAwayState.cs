using System;
using UnityEngine;
public class PassengerFlyingAwayState : MovableCharacterState
{
    private Vector3 _flyTarget;
    private const int FlyLength = 30;
    private PassengerSM _passenger;

    protected override void OnStart()
    {
        Vector3 pos = MovableCharacter.transform.position;
        _flyTarget = new Vector2(pos.x, pos.y + FlyLength);
        DropBonus();
    }

    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("attacked");
        Vector3 newPosition = Vector3.MoveTowards(MovableCharacter.transform.position, _flyTarget, 50 * Time.deltaTime);
        MovableCharacter.transform.position = newPosition;
        float sqrRemainingDistance = ((Vector2)newPosition - (Vector2)_flyTarget).sqrMagnitude;
        if (sqrRemainingDistance <= 1)
        {
            if (_passenger.IsFlyAwayListenerActivated())
            {
                MonobehaviorHandler.GetMonobeharior().GetObject<TrainingHandler>("TrainingHandler").ShowNext();
            }
            MonoBehaviour.Destroy(MovableCharacter.gameObject);
        }
    }

    public PassengerFlyingAwayState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }

    public override bool IsTransitionAllowed()
    {
        return false;
    }


    public void DropBonus()
    {
        if (!TrainingHandler.IsTrainingFinished())
        {
            if (
                !MonobehaviorHandler.GetMonobeharior()
                    .GetObject<TrainingHandler>("TrainingHandler")
                    .IsBonusDropEnabled())
            {
                return;
            }
        }
        if (MonobehaviorHandler.GetMonobeharior()
                .GetObject<BonusTimer>("bonusTimer")
                .IsAnyBonusActive())
            return;
        if (!Randomizer.GetPercentageBasedBoolean((int)_passenger.BonusProbability))
            return;
        GameController.BonusTypes bonusType = Randomizer.CalculateValue<GameController.BonusTypes>(_passenger.BonusProbabilities);
        IBonus drop = null;
        switch (bonusType)
        {
            case GameController.BonusTypes.Wheel:
                drop = new WheelBonus();
                break;
            case GameController.BonusTypes.Ticket:
                drop = new TicketBonus();
                break;
            case GameController.BonusTypes.Boot:
                drop = new BootBonus();
                break;
            case GameController.BonusTypes.Magnet:
                drop = new MagnetBonus();
                break;
            case GameController.BonusTypes.Smile:
                drop = new SmileBonus();
                break;
            case GameController.BonusTypes.AntiHare:
                drop = new AntiHareBonus();
                break;
            case GameController.BonusTypes.SandGlass:
                drop = new SandGlassBonus();
                break;
            case GameController.BonusTypes.Vortex:
                drop = new VortexBonus();
                break;
            case GameController.BonusTypes.Snow:
                drop = new SnowBonus();
                break;
            case GameController.BonusTypes.Wrench:
                drop = new WrenchBonus();
                break;
            case GameController.BonusTypes.Cogwheel:
                break;
            case GameController.BonusTypes.Heal:
                drop = new HealBonus();
                break;
            default:
                return;
        }
        if (drop != null)
        {
            MonobehaviorHandler.GetMonobeharior()
                .GetObject<BonusTimer>("bonusTimer").DropBonus(drop, _passenger.transform.position);
        }
    }
}
