using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 04/25/2024
 * Notes:   This provides a robust tag management system
 *          beyond the default provided by Unity.
 */

public class TagManager : MonoBehaviour
{
    public enum BaseTag
    {
        None,
        Enemy
    }

    public enum ResourceType
    {
        None,
        Butterfalls,
        Sugerpond,
        Flourmine
    }

    public BaseTag tagType = BaseTag.None;
    public ResourceType resourceType = ResourceType.None;
}
