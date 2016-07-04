using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent.StandardEvents
{
    /*
     * We want to factor out the code which checks for the existence of required objects, because they shouldn't even
     * be considered for valid actions if they don't exist.
     * 
     */
    public static class MarketAndInventoryEvents
    {
        public static List<GameAction> GameEvents = new List<GameAction>() { 
            new GameAction()
            {
                ID = "PurchaseSword",
                ShowOutcomes = false,
                Description = (world) => { return "Purchase a new sword."; },
                RequiredEntities = new List<String>() { "player" },
                IsValidDel = (world) => {
                    // This is valid if the player is at the market and not currently travelling somewhere else
                    return 
                        world.AllEntities["player"].S["Location"] == "market" &&
                        world.AllEntities["player"].S["Destination"] == null; 
                }
            },
            new GameAction()
            {
                ID = "PurchaseArmor",
                ShowOutcomes = false,
                Description = (world) => { return "Purchase some new armor."; },
                RequiredEntities = new List<String>() { "player" },
                IsValidDel = (world) => {
                    // This is valid if the player is at the market and not currently travelling somewhere else
                    return 
                        world.AllEntities["player"].S["Location"] == "market" &&
                        world.AllEntities["player"].S["Destination"] == null; 
                }
            },
        };

        public static List<Outcome> GameEventOutcomes = new List<Outcome>() { 
            new Outcome()
            {
                GetDescription = (source, world) => { return "Buy a sword"; },
                IsValid = (source, world) => {
                    return source.ID == "PurchaseSword";
                },
                PerformOutcome = (ref GameWorld world) => {
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("Player buys a sword");
                    return sbResult.ToString();
                }
            },
            new Outcome()
            {
                GetDescription = (source, world) => { return "Buy some armor"; },
                IsValid = (source, world) => {
                    return source.ID == "PurchaseArmor";
                },
                PerformOutcome = (ref GameWorld world) => {
                    StringBuilder sbResult = new StringBuilder();
                    sbResult.Append("Player buys some armor");
                    return sbResult.ToString();
                }
            }
        };
    }
}
