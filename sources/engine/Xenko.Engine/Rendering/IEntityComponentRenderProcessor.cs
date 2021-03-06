// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using Xenko.Engine;

namespace Xenko.Rendering
{
    /// <summary>
    /// An <see cref="EntityProcessor"/> dedicated for rendering.
    /// </summary>
    /// Note that it might be instantiated multiple times in a given <see cref="SceneInstance"/>.
    public interface IEntityComponentRenderProcessor
    {
        VisibilityGroup VisibilityGroup { get; set; }
    }
}
