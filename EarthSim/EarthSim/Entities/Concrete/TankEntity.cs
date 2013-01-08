using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EarthSim.Entities.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EarthSim.Entities.Concrete
{
    public class TankEntity : AbstractPlayerEntity
    {
        Model tankModel;
        AbstractSphereEntity Target;

        // Shortcut references to the bones that we are going to animate.
        // We could just look these up inside the Draw method, but it is more
        // efficient to do the lookups while loading and cache the results.
        ModelBone leftBackWheelBone;
        ModelBone rightBackWheelBone;
        ModelBone leftFrontWheelBone;
        ModelBone rightFrontWheelBone;
        ModelBone tankGeoBone;
        ModelBone leftSteerBone;
        ModelBone rightSteerBone;
        ModelBone turretBone;
        ModelBone cannonBone;
        ModelBone hatchBone;

        // Store the original transform matrix for each animating bone.
        Matrix leftBackWheelTransform;
        Matrix rightBackWheelTransform;
        Matrix leftFrontWheelTransform;
        Matrix rightFrontWheelTransform;
        Matrix tankGeoTransform;
        Matrix leftSteerTransform;
        Matrix rightSteerTransform;
        Matrix turretTransform;
        Matrix cannonTransform;
        Matrix hatchTransform;

        // Array holding all the bone transform matrices for the entire model.
        // We could just allocate this locally inside the Draw method, but it
        // is more efficient to reuse a single array, as this avoids creating
        // unnecessary garbage.
        Matrix[] boneTransforms;

        // Current animation positions.
        private float leftBackWheelRotationValue;
        private float rightBackWheelRotationValue;
        private float leftFrontWheelRotationValue;
        private float rightFrontWheelRotationValue;
        private float tankGeoRotationValue;
        //private float steerRotationValue;
        private float turretRotationValue;
        private float cannonRotationValue;
        private float hatchRotationValue;

        // Current elevations
        private float leftBackWheelElevation;
        private float rightBackWheelElevation;
        private float leftFrontWheelElevation;
        private float rightFrontWheelElevation;

        public TankEntity(Game game, Model model, AbstractSphereEntity target)
            : base(game)
        {
            this.tankModel = model;
            this.Position = new Vector3(0.0f, 0.0f, 0.0f);
            this.Target = target;

            /*tankModel.Root.Transform = Matrix.CreateScale(Scale)
                * Matrix.CreateRotationZ(MathHelper.ToRadians(90))
                * Matrix.CreateRotationX(MathHelper.ToRadians(-90))
                * Matrix.CreateTranslation(this.Position);*/

            //Heading = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(-90));

            // Look up shortcut references to the bones we are going to animate.
            leftBackWheelBone = tankModel.Bones["l_back_wheel_geo"];
            rightBackWheelBone = tankModel.Bones["r_back_wheel_geo"];
            leftFrontWheelBone = tankModel.Bones["l_front_wheel_geo"];
            rightFrontWheelBone = tankModel.Bones["r_front_wheel_geo"];
            tankGeoBone = tankModel.Bones["tank_geo"];
            leftSteerBone = tankModel.Bones["l_steer_geo"];
            rightSteerBone = tankModel.Bones["r_steer_geo"];
            turretBone = tankModel.Bones["turret_geo"];
            cannonBone = tankModel.Bones["canon_geo"];
            hatchBone = tankModel.Bones["hatch_geo"];

            //Rotation = Quaternion.Identity;
            //Height = Matrix.Identity;

            // Store the original transform matrix for each animating bone.
            leftBackWheelTransform = leftBackWheelBone.Transform;
            rightBackWheelTransform = rightBackWheelBone.Transform;
            leftFrontWheelTransform = leftFrontWheelBone.Transform;
            rightFrontWheelTransform = rightFrontWheelBone.Transform;
            tankGeoTransform = tankGeoBone.Transform;
            //leftSteerTransform = leftSteerBone.Transform;
            //rightSteerTransform = rightSteerBone.Transform;
            turretTransform = turretBone.Transform;
            cannonTransform = cannonBone.Transform;
            hatchTransform = hatchBone.Transform;

            // Allocate the transform matrix array.
            boneTransforms = new Matrix[tankModel.Bones.Count];
        }

        public override void Update(GameTime gameTime)
        {
            // Reset rotation values
            // TODO: If rotate
            leftBackWheelRotationValue = 0;
            rightBackWheelRotationValue = 0;
            leftFrontWheelRotationValue = 0;
            rightFrontWheelRotationValue = 0;

            if (geoLatitude > 90 || geoLatitude < -90) geoLatitude = 1f;
            if (geoLongitude > 90 || geoLongitude < -90) geoLongitude = 1f;

            // ...

            //leftFrontWheelElevation
            //rightFrontWheelElevation
            //leftBackWheelElevation
            //rightBackWheelElevation

            //xRotation
            //zRotation

            //elevation

            base.Update(gameTime);
        }

        public override void Draw(Matrix world, Matrix view, Matrix projection, BasicEffect effect)
        {
            DrawModel(tankModel, projection, view, world);
        }

        private void DrawModel(Model tankModel, Matrix projection, Matrix view, Matrix world)
        {
            Matrix rotationX = Matrix.CreateRotationX(xRotation);
            Matrix rotationZ = Matrix.CreateRotationZ(zRotation);

            Matrix tankModelRotation = Matrix.CreateRotationY(tankGeoRotationValue);
            Matrix leftBackTireRotation = Matrix.CreateRotationX(leftBackWheelRotationValue);
            Matrix rightBackTireRotation = Matrix.CreateRotationX(rightBackWheelRotationValue);
            Matrix leftFrontTireRotation = Matrix.CreateRotationX(leftFrontWheelRotationValue);
            Matrix rightFrontTireRotation = Matrix.CreateRotationX(rightFrontWheelRotationValue);
            //Matrix wheelRotation = Matrix.CreateRotationX(wheelRotationValue);
            //Matrix steerRotation = Matrix.CreateRotationY(steerRotationValue);
            Matrix turretRotation = Matrix.CreateRotationY(turretRotationValue);
            Matrix cannonRotation = Matrix.CreateRotationX(cannonRotationValue);
            Matrix hatchRotation = Matrix.CreateRotationX(hatchRotationValue);
            tankModel.Root.Transform = world * tankModelRotation * rotationX * rotationZ;


            leftBackWheelBone.Transform = leftBackTireRotation * leftBackWheelTransform;
            rightBackWheelBone.Transform = rightBackTireRotation * rightBackWheelTransform;
            leftFrontWheelBone.Transform = leftFrontTireRotation * leftFrontWheelTransform;
            rightFrontWheelBone.Transform = rightFrontTireRotation * rightFrontWheelTransform;
            //leftSteerBone.Transform = steerRotation * leftSteerTransform;
            //rightSteerBone.Transform = steerRotation * rightSteerTransform;
            turretBone.Transform = turretRotation * turretTransform;
            cannonBone.Transform = cannonRotation * cannonTransform;
            hatchBone.Transform = hatchRotation * hatchTransform;

            // Look up combined bone matrices for the entire model.
            tankModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            // Draw the model.
            foreach (ModelMesh mesh in tankModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();

                    Matrix tankRotation;
                    float pitch = 0;
                    float roll = 0;
                    float yaw = 0;

                    effect.EnableDefaultLighting();
                    effect.Projection = projection;
                    effect.View = view;
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    GetRotationForModel(out pitch, out roll, out yaw, geoLatitude, geoLongitude);
                    Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out Rotation);
                    Matrix.CreateFromQuaternion(ref Rotation, out tankRotation);

                    effect.World = boneTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(100f) * tankRotation *
                        Matrix.CreateTranslation(Target.GetGeoPosition(geoLatitude, geoLongitude, geoElevation));
                }

                mesh.Draw();
            }
        }
    }
}
