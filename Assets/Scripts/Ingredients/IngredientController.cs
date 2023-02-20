using UnityEngine;

namespace Ingredients
{
    public class IngredientController : MonoBehaviour
    {
        public void CollectIngredient()
        {
            //TODO: Add sound effects etc.
            gameObject.SetActive(false);
        }
    }
}
