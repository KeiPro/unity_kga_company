using UnityEngine;

public interface ICharacter
{
    int Level { get; set; }
    int HP { get; set; }

    int Stamina { get; set; }
    int MaxHP { get; set; }
    int MaxStamina { get; set; }

    Animator animator { get; set; }



}
