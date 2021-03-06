﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EarthSim.Entities.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EarthSim.Entities.Concrete
{
    public class BasicTankEntity : AbstractPlayerEntity
    {
        #region Fields

        AbstractSphereEntity _target;

        // The XNA framework Model object that we are going to display.
        Model tankModel;

        // Shortcut references to the bones that we are going to animate.
        // We could just look these up inside the Draw method, but it is more
        // efficient to do the lookups while loading and cache the results.
        ModelBone leftBackWheelBone;
        ModelBone rightBackWheelBone;
        ModelBone leftFrontWheelBone;
        ModelBone rightFrontWheelBone;
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
        float wheelRotationValue;
        float steerRotationValue;
        float turretRotationValue;
        float cannonRotationValue;
        float hatchRotationValue;

        // Current elevations
        private float leftBackWheelElevation;
        private float rightBackWheelElevation;
        private float leftFrontWheelElevation;
        private float rightFrontWheelElevation;

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets the wheel rotation amount.
        /// </summary>
        public float WheelRotation
        {
            get { return wheelRotationValue; }
            set { wheelRotationValue = value; }
        }

        /// <summary>
        /// Gets or sets the steering rotation amount.
        /// </summary>
        public float SteerRotation
        {
            get { return steerRotationValue; }
            set { steerRotationValue = value; }
        }

        /// <summary>
        /// Gets or sets the turret rotation amount.
        /// </summary>
        public float TurretRotation
        {
            get { return turretRotationValue; }
            set { turretRotationValue = value; }
        }

        /// <summary>
        /// Gets or sets the cannon rotation amount.
        /// </summary>
        public float CannonRotation
        {
            get { return cannonRotationValue; }
            set { cannonRotationValue = value; }
        }

        /// <summary>
        /// Gets or sets the entry hatch rotation amount.
        /// </summary>
        public float HatchRotation
        {
            get { return hatchRotationValue; }
            set { hatchRotationValue = value; }
        }

        #endregion

        public BasicTankEntity(Game game, Model model, AbstractSphereEntity target)
            : base(game)
        {
            this.tankModel = model;
            this._target = target;
            this.Scale = 0.001f;
            // Sweden
            geoLatitude = 57;
            geoLongitude = 14;

            // Look up shortcut references to the bones we are going to animate.
            leftBackWheelBone = tankModel.Bones["l_back_wheel_geo"];
            rightBackWheelBone = tankModel.Bones["r_back_wheel_geo"];
            leftFrontWheelBone = tankModel.Bones["l_front_wheel_geo"];
            rightFrontWheelBone = tankModel.Bones["r_front_wheel_geo"];
            leftSteerBone = tankModel.Bones["l_steer_geo"];
            rightSteerBone = tankModel.Bones["r_steer_geo"];
            turretBone = tankModel.Bones["turret_geo"];
            cannonBone = tankModel.Bones["canon_geo"];
            hatchBone = tankModel.Bones["hatch_geo"];

            // Store the original transform matrix for each animating bone.
            leftBackWheelTransform = leftBackWheelBone.Transform;
            rightBackWheelTransform = rightBackWheelBone.Transform;
            leftFrontWheelTransform = leftFrontWheelBone.Transform;
            rightFrontWheelTransform = rightFrontWheelBone.Transform;
            leftSteerTransform = leftSteerBone.Transform;
            rightSteerTransform = rightSteerBone.Transform;
            turretTransform = turretBone.Transform;
            cannonTransform = cannonBone.Transform;
            hatchTransform = hatchBone.Transform;

            // Allocate the transform matrix array.
            boneTransforms = new Matrix[tankModel.Bones.Count];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float smoothness = 0.01f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            geoElevation = _target.GetLocalElevation(geoLatitude, geoLongitude, geoElevation, smoothness);

            // Distance between wheels and center of tank
            float wheelOffset = 1f;
            leftBackWheelElevation = _target.GetLocalElevation(geoLatitude + wheelOffset, geoLongitude - wheelOffset, leftBackWheelElevation, smoothness);
            leftFrontWheelElevation = _target.GetLocalElevation(geoLatitude + wheelOffset, geoLongitude + wheelOffset, leftFrontWheelElevation, smoothness);
            rightBackWheelElevation = _target.GetLocalElevation(geoLatitude - wheelOffset, geoLongitude - wheelOffset, rightBackWheelElevation, smoothness);
            rightFrontWheelElevation = _target.GetLocalElevation(geoLatitude - wheelOffset, geoLongitude + wheelOffset, rightFrontWheelElevation, smoothness);

            // X rotation is (LB + RB) - (LF + RF)
            xRotation = (leftBackWheelElevation + rightBackWheelElevation)
                        - (leftFrontWheelElevation + rightFrontWheelElevation);
            // Z rotation is (RB + RF) - (LB + LF)
            zRotation = (rightBackWheelElevation + rightFrontWheelElevation)
                        - (leftBackWheelElevation + leftFrontWheelElevation);
        }

        public override void Draw(Matrix world, Matrix view, Matrix projection, BasicEffect effect)
        {
            DrawModel(world, view, projection);
        }

        private void DrawModel(Matrix world, Matrix view, Matrix projection)
        {
            // Set the world matrix as the root transform of the model.
            tankModel.Root.Transform = world;

            // Calculate matrices based on the current animation position.
            Matrix wheelRotation = Matrix.CreateRotationX(wheelRotationValue);
            Matrix steerRotation = Matrix.CreateRotationY(steerRotationValue);
            Matrix turretRotation = Matrix.CreateRotationY(turretRotationValue);
            Matrix cannonRotation = Matrix.CreateRotationX(cannonRotationValue);
            Matrix hatchRotation = Matrix.CreateRotationX(hatchRotationValue);

            // Apply matrices to the relevant bones.
            leftBackWheelBone.Transform = wheelRotation * leftBackWheelTransform;
            rightBackWheelBone.Transform = wheelRotation * rightBackWheelTransform;
            leftFrontWheelBone.Transform = wheelRotation * leftFrontWheelTransform;
            rightFrontWheelBone.Transform = wheelRotation * rightFrontWheelTransform;
            leftSteerBone.Transform = steerRotation * leftSteerTransform;
            rightSteerBone.Transform = steerRotation * rightSteerTransform;
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
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();

                    Matrix tankRotation;
                    float pitch = 0;
                    float roll = 0;
                    float yaw = 0;
                    
                    GetRotationForModel(out pitch, out roll, out yaw, geoLatitude, geoLongitude);
                    Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out Rotation);
                    Matrix.CreateFromQuaternion(ref Rotation, out tankRotation);
                    this.Position = _target.GetGeoPosition(geoLatitude, geoLongitude, geoElevation);

                    effect.World = boneTransforms[mesh.ParentBone.Index]
                        * Matrix.CreateScale(Scale)
                        * Matrix.CreateFromAxisAngle(Vector3.UnitX, xRotation)
                        * Matrix.CreateFromAxisAngle(Vector3.UnitZ, -zRotation) 
                        * tankRotation
                        * Matrix.CreateTranslation(Position);
                }

                mesh.Draw();
            }
        }
    }
}
