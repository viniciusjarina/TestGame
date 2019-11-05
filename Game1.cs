using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Effect effect;
        private Effect blurEffect;
        private Texture2D texture;
        private SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        RenderTarget2D target1;
        RenderTarget2D target2;
        GaussianBlur blur;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = Content.Load<Effect>("File");
            blurEffect = Content.Load<Effect>("Blur");
            texture = Content.Load<Texture2D>("troll");
            font = Content.Load<SpriteFont>("FantasDefaultFont");

            blur = new GaussianBlur(blurEffect);
            blur.BlurAmount = 1;

            basicEffect = new BasicEffect(GraphicsDevice);
            var view       = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
            var projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, -1);


            basicEffect.World = Matrix.Identity;
            basicEffect.View = view;
            basicEffect.Projection = projection;

            basicEffect.VertexColorEnabled = false;

            target1 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            target2 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            VertexPosition [] vertices = new VertexPosition[3] 
            {
                new VertexPosition(new Vector3(0,50,0)),
                new VertexPosition(new Vector3(0,0,0)),
                new VertexPosition(new Vector3(50,0,0))
            };

            buffer1 = new VertexBuffer(GraphicsDevice, typeof(VertexPosition), 3, BufferUsage.WriteOnly);
            buffer1.SetData(vertices);

            //var vertices2 = new VertexPosition[3] 
            //{
            //    new VertexPosition(new Vector3(100,10,0)),
            //    new VertexPosition(new Vector3(220,300,0)),
            //    new VertexPosition(new Vector3(450,10,0))
            //};

            //buffer2 = new VertexBuffer(GraphicsDevice, typeof(VertexPosition), 3, BufferUsage.WriteOnly);
            //buffer2.SetData(vertices2);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        VertexBuffer buffer1;
        //VertexBuffer buffer2;
        BasicEffect basicEffect;
        float rotation = 0.0f;
        float alpha = 0.0f;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(target1);

            GraphicsDevice.Clear(Color.CornflowerBlue);


            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            BlendState blendState = BlendState.NonPremultiplied;
            SamplerState samplerState = SamplerState.LinearClamp;
            DepthStencilState depthStencilState = DepthStencilState.None;
            RasterizerState rasterizerState = RasterizerState.CullNone;
            Effect xeffect = null;
            

            Matrix transform = Matrix.CreateScale(1, -1, 1) * 
                Matrix.CreateTranslation(00, GraphicsDevice.Viewport.Height, 0)
                ;
            
            ///public void Draw(Texture2D texture, 
            ///                 Vector2 position,
            ///                 Rectangle? sourceRectangle,
            ///                 Color color,
            ///                 float rotation,
            ///                 Vector2 origin,
            ///                 Vector2 scale,
            ///                 SpriteEffects effects,
            ///                 float layerDepth);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, xeffect, transform);
            spriteBatch.Draw(texture,
                             new Vector2(0, 0),
                             null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.FlipVertically, 0f);

            
            spriteBatch.End();

           // effect.
            //basicEffect.CurrentTechnique = effect.CurrentTechnique;
            GraphicsDevice.SetVertexBuffer(buffer1);
            basicEffect.World = Matrix.Identity * transform;
            //basicEffect.World = Matrix.CreateRotationZ(rotation);

            alpha += 0.01f;
            if (alpha > 1f)
                alpha = 0.0f;

            basicEffect.Alpha = 1f;
            basicEffect.VertexColorEnabled = false;
            basicEffect.DiffuseColor = new Vector3(1, 0, 0);
            basicEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 1);

//            GraphicsDevice.SetVertexBuffer(buffer1);
            basicEffect.World = Matrix.CreateTranslation(100, 100 , 0) * transform;
            basicEffect.Alpha = 1f;
            basicEffect.VertexColorEnabled = false;
            basicEffect.DiffuseColor = new Vector3(0, 0, 1);
            //basicEffect.EnableDefaultLighting();
            basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 1);

            basicEffect.World = Matrix.CreateTranslation(200, 200 , 0) * transform;
            basicEffect.Alpha = 1f;
            basicEffect.VertexColorEnabled = false;
            basicEffect.DiffuseColor = new Vector3(1, 1, 0);
            //basicEffect.EnableDefaultLighting();
            basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 1);

           // xeffect = effect;
            //GraphicsDevice.SetRenderTarget(target1);
            //xeffect = blurEffect;
            //rotation += 0.01f;
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, xeffect, transform);
            spriteBatch.DrawString(font, "LALALALALAL", new Vector2(200, 700), Color.Yellow, MathHelper.PiOver4, new Vector2(200, 700), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(texture,
                             new Vector2(100, 100),
                             null, new Color(Color.Green, 0.5f), 0f, Vector2.Zero, Vector2.One, SpriteEffects.FlipVertically, 0f);
            spriteBatch.Draw(texture,
                             new Vector2(200, 200),
                             null, new Color(Color.Blue, 0.5f), 0f, Vector2.Zero, Vector2.One, SpriteEffects.FlipVertically, 0f);
            spriteBatch.End();





            //GraphicsDevice.SetRenderTarget(target2);

            //spriteBatch.Begin(effect: blurEffect);
            //spriteBatch.Draw(target1, Vector2.Zero, Color.White);
            //spriteBatch.End();

            //GraphicsDevice.SetRenderTarget(target2);

            //spriteBatch.Begin(effect: effect);
            //spriteBatch.Draw(target1, Vector2.Zero, Color.White);
            //spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            sortMode = SpriteSortMode.Immediate;
            blendState = BlendState.Opaque;
            samplerState = SamplerState.LinearClamp;
            depthStencilState = DepthStencilState.None;
            rasterizerState = RasterizerState.CullNone;

            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState);
            spriteBatch.Draw(target1, Vector2.Zero, Color.White);
            spriteBatch.End();


            // TODO:  

            base.Draw(gameTime);
        }
    }
}
