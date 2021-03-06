// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using Xenko.Core.Collections;
using Xenko.Core.Mathematics;
using Xenko.Core.Storage;
using Xenko.Engine;

namespace Xenko.Rendering.LightProbes
{
    public class LightProbeProcessor : EntityProcessor<LightProbeComponent>
    {
        private ObjectId previousLightProbeHash;
        private bool needPositionUpdate = false;

        public LightProbeProcessor() : base(typeof(TransformComponent))
        {
        }

        /// <summary>
        /// The current light probe runtime data.
        /// </summary>
        public LightProbeRuntimeData RuntimeData { get; private set; }

        /// <summary>
        /// Light probe runtime data is auto-computed when lightprobes are added/removed.  If you move them at runtime, please call this method.
        /// </summary>
        /// <remarks>
        /// This will also update coefficients.
        /// </remarks>
        public void UpdateLightProbePositions()
        {
            RuntimeData = null;
            needPositionUpdate = false;

            // Initial load
            try
            {
                // Collect LightProbes
                var lightProbes = new FastList<LightProbeComponent>();

                foreach (var lightProbe in ComponentDatas)
                {
                    lightProbes.Add(lightProbe.Key);
                }

                // Need at least 4 light probes to form a tetrahedron
                if (lightProbes.Count < 4)
                    return;

                RuntimeData = LightProbeGenerator.GenerateRuntimeData(lightProbes);
            }
            catch
            {
                // Allow failures
                // TODO: Log
            }
        }

        /// <summary>
        /// Updates only the coefficients of the light probes (from <see cref="LightProbeComponent.Coefficients"/> to <see cref="LightProbeRuntimeData.Coefficients"/>).
        /// </summary>
        public void UpdateLightProbeCoefficients()
        {
            if (RuntimeData == null)
                return;

            LightProbeGenerator.UpdateCoefficients(RuntimeData);
        }

        public override void Draw(RenderContext context)
        {
            if (needPositionUpdate)
            {
                UpdateLightProbePositions();
            }
        }

        protected override void OnEntityComponentAdding(Entity entity, LightProbeComponent component, LightProbeComponent data)
        {
            needPositionUpdate = true;
        }

        protected override void OnEntityComponentRemoved(Entity entity, LightProbeComponent component, LightProbeComponent data)
        {
            needPositionUpdate = true;
        }
    }
}
