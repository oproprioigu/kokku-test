using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string Name { get; set; }
        public float Health;
        public float BaseDamage;
        public float DamageMultiplier { get; set; }
        public GridBox currentBox;
        public int PlayerIndex;
        public Random Rand = new Random();
        public Character Target { get; set; } 
        public Character(CharacterClass characterClass)
        {

        }


        public bool TakeDamage(float amount)
        {
            if((Health -= BaseDamage) <= 0)
            {
                Die();
                return true;
            }
            return false;
        }

        public void Die()
        {
            //TODO >> maybe kill him?
        }

        public void WalkTO(bool CanWalk)
        {

        }

        public void StartTurn(Grid battlefield)
        {

            if (CheckCloseTargets(battlefield))
            {
                Attack(Target);


                return;
            }
            else
            {   // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                bool enemyIsAtRight = currentBox.xIndex < Target.currentBox.xIndex;
                bool enemyIsAtLeft = currentBox.xIndex > Target.currentBox.xIndex;
                bool enemyIsAbove = currentBox.yIndex > Target.currentBox.yIndex;
                bool enemyIsBelow = currentBox.yIndex < Target.currentBox.yIndex;

                int rightIndexClamped = Math.Min(currentBox.Index + 1, (battlefield.yLength * battlefield.xLength) - 1);
                int leftIndexClamped = Math.Max(currentBox.Index - 1, 0);
                int upIndexClamped = Math.Max(currentBox.Index - battlefield.yLength, 0); 
                int downIndexClamped = Math.Min(currentBox.Index + battlefield.yLength, (battlefield.yLength * battlefield.xLength) - 1);

                bool rightNeighborIsOccupied = battlefield.grids[rightIndexClamped].ocupied;
                bool leftNeighborIsOccupied = battlefield.grids[leftIndexClamped].ocupied;
                bool upNeighborIsOccupied = battlefield.grids[upIndexClamped].ocupied;
                bool downNeighborIsOccupied = battlefield.grids[downIndexClamped].ocupied;

                if (enemyIsAtRight && !rightNeighborIsOccupied)
                {
                    var cel = battlefield.grids.Find(x => x.Index == rightIndexClamped);
                    var index = cel.Index;
                    OccupySpace(battlefield, index);

                    Console.WriteLine($"Player {PlayerIndex} walked right\n");
                    battlefield.drawBattlefield();

                    return;
                }
                else if (enemyIsAtLeft && !leftNeighborIsOccupied)
                {
                    var cel = battlefield.grids.Find(x => x.Index == leftIndexClamped);
                    var index = cel.Index;
                    OccupySpace(battlefield, index);

                    Console.WriteLine($"Player {PlayerIndex} walked left\n");
                    battlefield.drawBattlefield();

                    return;
                }

                if (enemyIsAbove && !upNeighborIsOccupied)
                {
                    var cel = battlefield.grids.Find(x => x.Index == upIndexClamped);
                    var index = cel.Index;
                    OccupySpace(battlefield, index);
                    Console.WriteLine($"Player {PlayerIndex} walked up\n");
                    battlefield.drawBattlefield();

                    return;
                }
                else if (enemyIsBelow && !downNeighborIsOccupied)
                {
                    var cel = battlefield.grids.Find(x => x.Index == downIndexClamped);
                    var index = cel.Index;
                    OccupySpace(battlefield, index);
                    Console.WriteLine($"Player {PlayerIndex} walked down\n");
                    battlefield.drawBattlefield();

                    return;
                }
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLength).ocupied);
            bool down = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLength).ocupied);

            if (left || right || up || down) 
            {
                return true;
            }
            return false; 
        }

        public void Attack (Character target)
        {
            var damageDealt = Rand.Next(0, (int)BaseDamage);
            target.TakeDamage(damageDealt);
            Console.WriteLine($"Player {PlayerIndex} is attacking the player {Target.PlayerIndex} and did {damageDealt} damage\n");
        }

        public void OccupySpace(Grid grid, int index)
        {
            this.currentBox.ocupied = false;
            grid.grids[currentBox.Index] = this.currentBox;
            this.currentBox = grid.grids[index];
            this.currentBox.ocupied = true;
            grid.grids[currentBox.Index] = this.currentBox;
        }

    }
}
