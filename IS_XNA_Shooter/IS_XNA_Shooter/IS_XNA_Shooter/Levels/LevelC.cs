﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace IS_XNA_Shooter
{
    class LevelC : Level
    {

        private Texture2D whitePixel;
        private Texture2D textureCell;
        private float globalTime;
        private float globalTimeAux;
        private float timeLapse;

        XmlNodeList level;
        private int numLevel;
        private int numLevelAux;

        private bool testingEnemies;
        bool havePassed;

  

        public LevelC(Camera camera, int num, List<Enemy> enemies)
            : base(camera, num, enemies)
        {
            testingEnemies = false;
            globalTime = 0;
            globalTimeAux = 0;
            timeLapse = 2;

            numLevel = 2;
            numLevelAux = 2;
           
            havePassed = false;

            switch (num)
            {
                case 0: // Level for testing enemies
                    width = 1200;
                    height = 800;
                    ShipInitPosition = new Vector2(width / 2, height / 2);
                    this.enemies = enemies;

                    testingEnemies = true;

                    break;

                case 1:
                    width = 1200;
                    height = 800;
                    ShipInitPosition = new Vector2(width / 2, height / 2);
                    this.enemies = enemies;

                    LeerArchivoXML();

                    break;
            }

            whitePixel = GRMng.whitepixel;
            textureCell = GRMng.textureCell;
      
        }

        public override void Update(float deltaTime)
        {

            int i = 0; // iterator for the list of enemies
            bool stillAlive = false; // is true if there is any enemie alive
            //the next loop searches an enemy alive for controlling the end of level 

            if (globalTimeAux > 310 && numLevelAux != 0 && timeLapse <= 0)
            {
                
                LeerArchivoXML();
                timeLapse = 2;
                numLevelAux--;
                

                
            }
            else  if (numLevelAux == 0) {
                             numLevel++;
                             numLevelAux = numLevel;
                             globalTimeAux = 310;
                             timeLapse = 2;
                         } 
                                        
           


            globalTime = globalTime + deltaTime;
            globalTimeAux = globalTimeAux + deltaTime;
            timeLapse = timeLapse - deltaTime;

            if (!levelFinished)
            {
                while (i < enemies.Count && !stillAlive)
                {
                    if (enemies[i] != null && !enemies[i].isDead())
                    {
                        stillAlive = true;
                    }
                    i++;
                }
                if (!stillAlive)
                    levelFinished = true;
            }

            if (testingEnemies)
            {
                TestEnemies();
            }

        } // Update

        public override void Draw(SpriteBatch spriteBatch)
        {
            // grid del suelo
            for (int i = 0; i < width; i += textureCell.Width)
                for (int j = 0; j < height; j += textureCell.Height)
                    spriteBatch.Draw(textureCell, new Vector2(i + camera.displacement.X, j + camera.displacement.Y),
                        Color.White);

            // linea de arriba:
            spriteBatch.Draw(whitePixel, new Rectangle((int)camera.displacement.X, (int)camera.displacement.Y,
                width, 1), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            // linea de la derecha:
            spriteBatch.Draw(whitePixel, new Rectangle((int)camera.displacement.X, (int)camera.displacement.Y,
                1, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            // linea de abajo:
            spriteBatch.Draw(whitePixel, new Rectangle((int)camera.displacement.X + width, (int)camera.displacement.Y,
                1, height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            // linea de la izquierda:
            spriteBatch.Draw(whitePixel, new Rectangle((int)camera.displacement.X, (int)camera.displacement.Y + height,
                width, 1), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

            //Pintamos el tiempo transcurrido del juego

            spriteBatch.DrawString(SuperGame.fontMotorwerk, "Time with live: ",
                new Vector2(800, 15), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.DrawString(SuperGame.fontMotorwerk, Math.Truncate(10 *globalTime) / 10 + " seconds",
                new Vector2(980, 35), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public override void setShip(Ship ship)
        {
            base.setShip(ship);
        }

        private void TestEnemies()
        {
            Enemy enemy;

            // EnemyWeak:
            if (ControlMng.f1Preshed)
            {
                enemy = EnemyFactory.GetEnemyByName("EnemyWeakA", camera, this, ship, new Vector2(20, 20), 0,null);
                enemies.Add(enemy);
            }

            // EnemyWeakShot:
            if (ControlMng.f2Preshed)
            {
                enemy = EnemyFactory.GetEnemyByName("EnemyWeakShotA", camera, this, ship, new Vector2(20, 20), 0, null);
                enemies.Add(enemy);
            }

            // EnemyBeamA:
            if (ControlMng.f3Preshed)
            {
                enemy = EnemyFactory.GetEnemyByName("EnemyBeamA", camera, this, ship, new Vector2(60, 60), 0, null);
                enemies.Add(enemy);
            }

            // EnemyMineShotA
            if (ControlMng.f4Preshed)
            {
                enemy = EnemyFactory.GetEnemyByName("EnemyMineShotA", camera, this, ship, new Vector2(20, 20), 0, null);
                enemies.Add(enemy);
            }

            // EnemyLaserA
            if (ControlMng.f5Preshed)
            {
                enemy = EnemyFactory.GetEnemyByName("EnemyLaserA", camera, this, ship, new Vector2(60, 60), 0, null);
                enemies.Add(enemy);
            }

            // EnemyScaredA
            if (ControlMng.f6Preshed)
            {
                enemy = EnemyFactory.GetEnemyByName("EnemyMineShotA", camera, this, ship, new Vector2(60, 60), 0, null);
                enemies.Add(enemy);
            }

            // Final Boss 1 Phase 4
            if (ControlMng.f7Preshed)
            {
                enemy = new FinalBoss1Turret1(camera, this, new Vector2(60, 60), ship);
                enemies.Add(enemy);
            }

            // Final Boss 1 Phase 4
            if (ControlMng.f8Preshed)
            {
                enemy = new FinalBoss1Turret2(camera, this, new Vector2(60, 60), ship);
                enemies.Add(enemy);
            }

            // Final Boss 1 Phase 4
            if (ControlMng.f9Preshed)
            {
                enemy = EnemyFactory.GetEnemyByName("FinalBossHeroe1", camera, this, ship, new Vector2(60, 60), 0, null);
                ((FinalBossHeroe1)enemy).SetEnemies(enemies);
                enemies.Add(enemy);
            }

        } // TestEnemies

        protected void LeerArchivoXML()
        {
            // Utilizar nombres de fichero y nodos XML idénticos a los que se guardaron

            try
            {

               //  Leer los datos del archivo
                String enemyType;
                float positionX;
                float positionY;
                float time;
                XmlDocument lvl = null;
                

                XMLLvlMng.lvl1C = new XmlDocument();
                XMLLvlMng.lvl1C.Load("../../../../IS_XNA_ShooterContent/Levels/level1C.xml");
                lvl = XMLLvlMng.lvl1C;
                  if (!havePassed)
                  {
                level = lvl.GetElementsByTagName("level");
                   havePassed = true;
                }
                XmlNodeList lista = null;
                lista = ((XmlElement)level[0]).GetElementsByTagName("enemy");

             
                

                foreach (XmlElement nodo in lista)
                {

                    XmlAttributeCollection enemyN = nodo.Attributes;
                    //XmlAttribute a = enemyN[1];
                    enemyType = Convert.ToString(enemyN[0].Value);
                    positionX = (float)Convert.ToDouble(enemyN[1].Value);
                    positionY = (float)Convert.ToDouble(enemyN[2].Value);
                    time = (float)Convert.ToDouble(enemyN[3].Value);
                    //timeLeftEnemy.Add(time);

                    Enemy enemy = null;


                    if (enemyType.Equals("enemyWeakA"))
                        enemy = new EnemyWeakA(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthEW1, GRMng.frameHeightEW1, GRMng.numAnimsEW1, GRMng.frameCountEW1,
                            GRMng.loopingEW1, SuperGame.frameTime12, GRMng.textureEW1, time, 100, 100,
                            1, ship);
                    else if (enemyType.Equals("enemyBeam"))
                        enemy = new EnemyBeamA(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthEB1, GRMng.frameHeightEB1, GRMng.numAnimsEB1, GRMng.frameCountEB1,
                            GRMng.loopingEB1, SuperGame.frameTime12, GRMng.textureEB1, time, 1000, 100,
                            4, ship);
                    else if (enemyType.Equals("enemyWeakShotA"))
                        enemy = new EnemyWeakShotA(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthEW2, GRMng.frameHeightEW2, GRMng.numAnimsEW2, GRMng.frameCountEW2,
                            GRMng.loopingEW2, SuperGame.frameTime12, GRMng.textureEW2, time, 100, 100,
                            1, ship, 2, 300, 200);
                    else if (enemyType.Equals("enemyMineShotA"))
                        enemy = new EnemyMineShotA(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthEMS, GRMng.frameHeightEMS, GRMng.numAnimsEMS, GRMng.frameCountEMS,
                            GRMng.loopingEMS, SuperGame.frameTime12, GRMng.textureEMS, time, 100, 100,
                            1, ship, 4, 140, 200);
                    else if (enemyType.Equals("enemyLaserA"))
                        enemy = new EnemyLaserA(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthEL, GRMng.frameHeightEL, GRMng.numAnimsEL, GRMng.frameCountEL,
                            GRMng.loopingEL, SuperGame.frameTime12, GRMng.textureEL, time, 100, 100,
                            1, ship);
                    else if (enemyType.Equals("enemyScaredA"))
                        enemy = new EnemyScaredA(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthES, GRMng.frameHeightES, GRMng.numAnimsES, GRMng.frameCountES,
                            GRMng.loopingES, SuperGame.frameTime12, GRMng.textureES, time, 100, 100,
                            1, ship, 4, 300, 200);
                    else if (enemyType.Equals("enemyWeakB"))
                        enemy = new EnemyWeakB(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthEW1, GRMng.frameHeightEW1, GRMng.numAnimsEW1, GRMng.frameCountEW1,
                            GRMng.loopingEW1, SuperGame.frameTime12, GRMng.textureEW1, time, 100, 100,
                            1, ship);
                    else if (enemyType.Equals("enemyWeakShotB"))
                        enemy = new EnemyWeakShotB(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthEW2, GRMng.frameHeightEW2, GRMng.numAnimsEW2, GRMng.frameCountEW2,
                            GRMng.loopingEW2, SuperGame.frameTime12, GRMng.textureEW2, time, 100, 100,
                            1, ship, 2, 300, 200);
                    else if (enemyType.Equals("enemyMineShotB"))
                        enemy = new EnemyMineShotB(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthEMS, GRMng.frameHeightEMS, GRMng.numAnimsEMS, GRMng.frameCountEMS,
                            GRMng.loopingEMS, SuperGame.frameTime12, GRMng.textureEMS, time, 100, 100,
                            1, ship, 4, 140, 200);
                    else if (enemyType.Equals("enemyLaserB"))
                        enemy = new EnemyLaserB(camera, this, new Vector2(positionX, positionY), 0,
                            GRMng.frameWidthEL, GRMng.frameHeightEL, GRMng.numAnimsEL, GRMng.frameCountEL,
                            GRMng.loopingEL, SuperGame.frameTime12, GRMng.textureEL, time, 100, 100,
                            1, ship);
                    /*  else if (enemyType.Equals("FinalBossHeroe1")) 
                       //   enemy = new FinalBossHeroe1(camera, this, new Vector2(positionX, positionY), ship, enemies);
                    
                    
                      */


                    enemies.Add(enemy);

                }

            }
            catch (Exception e)
            {
            }
        }   //  end LeerArchivoXML()

    } // class LevelA
}