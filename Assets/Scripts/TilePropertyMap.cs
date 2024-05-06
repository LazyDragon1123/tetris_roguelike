
using System.Collections.Generic;
using UnityEngine;

public class TilePropertyMap : MonoBehaviour
{
    private Dictionary<Vector3Int, TileProperty> properties = new Dictionary<Vector3Int, TileProperty>();

    // Set the property for a specific tile position
    public void SetTile(Vector3Int position, TileProperty property)
    {
        properties[position] = property;
    }

    // Get the property of a specific tile position
    public TileProperty GetTileProperty(Vector3Int position)
    {
        if (properties.ContainsKey(position))
        {
            return properties[position];
        }
        return null; // Or return a default property if you prefer
    }

    // Example method to mark a tile as special
    public void MarkTileAsSpecial(Vector3Int position)
    {
        SetTile(position, new TileProperty { isSpecial = true });
    }

    // Example method to check if a tile is special
    public bool IsTileSpecial(Vector3Int position)
    {
        TileProperty property = GetTileProperty(position);
        if (property != null)
        {
            return property.isSpecial;
        }
        return false;
    }

    public bool IsTileAttackable(Vector3Int position)
    {
        TileProperty property = GetTileProperty(position);
        if (property != null)
        {
            return property.isAttackable;
        }
        return false;
    }

    public void ClearAllTiles()
    {
        properties.Clear();
    }
}
