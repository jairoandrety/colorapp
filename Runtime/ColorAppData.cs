using System.Collections.Generic;
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [CreateAssetMenu(fileName = "ColorAppData", menuName = "ColorApp/ColorAppData")]
    public class ColorAppData : ScriptableObject
    {
        [SerializeField] private List<string> _colorLabels;
        public List<string> ColorLabels => _colorLabels;
        public void SetColorLabels(List<string> labels)
        {
            _colorLabels = labels;
        }
    }
}