using UnityEngine;

namespace Ingredients
{
    public class IngredientController : MonoBehaviour
    {
        public enum IngredientType
        {
            Chicken,
            Butter,
            Tomato,
            Garlic,
            Onion,
            Cream
        }

        public IngredientType ingredientType;
        
        public void CollectIngredient()
        {
            gameObject.SetActive(false);
        }
    }
}
