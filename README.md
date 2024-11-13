# Computer Graphics

## Final Demo Shaders

This is my final project for the Computer Graphics class, where I aimed to create three different effects: one inspired by the "ki charging" from Dragon Ball but with electric effects, a spell that casts a fiery slash leaving a trail, and finally, a ground slam that feels powerful.



### Power Charge

In brief, this effect relies on four main components: the ground effect, created using a concave 3D plane model and a texture shader; particle effects, including dust and small electric sparks; a hand-drawn electricity flipbook; and lightning bolts generated via script. The script creates start and end points for a line renderer, adding randomness and noise to simulate a lightning bolt.

![image](https://github.com/user-attachments/assets/75f2c319-a00c-4bae-9f8e-fc6c580daf63)

![image](https://github.com/user-attachments/assets/77c78704-6c3a-4b26-b28a-b277099cef27)

![image](https://github.com/user-attachments/assets/38e6465c-0e0d-4c72-bf3c-962f8288bb15)

**Lightning Script:**
```C#
void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.positionCount = pointCount;
        lineRenderer.material = material;
        

        StartCoroutine(ToggleLightning());
    }

    void Update()
    {
        UpdateThickness(thickness);
    }

    private IEnumerator ToggleLightning()
    {
        while (true)
        {
            lineRenderer.enabled = !lineRenderer.enabled;

            if (lineRenderer.enabled)
            {
                UpdateLightningPositions();
            }

            yield return new WaitForSeconds(toggleInterval);
        }
    }

    private void UpdateLightningPositions()
    {
        lineRenderer.SetPosition(0, startPoint.transform.position);
        lineRenderer.SetPosition(pointCount - 1, endPoint.transform.position);

        step = (endPoint.transform.position - startPoint.transform.position) / (pointCount - 1);

        for (int i = 1; i < pointCount - 1; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-randomness, randomness),
                Random.Range(-randomness, randomness),
                Random.Range(-randomness, randomness)
            );
            lineRenderer.SetPosition(i, startPoint.transform.position + step * i + randomOffset);
        }
    }

    private void UpdateThickness(float value)
    {
        lineRenderer.widthMultiplier = value;
    }
```

### Fire Spell

The fire spell has two parts: the "pre-cast" phase, where two particle systems work together—one for a flickering orb and others for smaller orbiting orbs to suggest spell summoning.

![image](https://github.com/user-attachments/assets/9d9d6575-47e6-4965-8b62-d8db87c29632)

The second phase, the invocation, uses three particle systems to add impact, with emissive trails or splashes of particles. The "Fire Slash" itself consists of two visual effects and a script. One visual effect generates a growing mesh for the slash that burns particles on the ground as it moves, giving the impression of scorched terrain. The other is a fire and smoke effect that leaves a trail. The script controls movement, drag, slowing over time, and invokes the fire slash.

![image](https://github.com/user-attachments/assets/87ac8942-64be-46a4-a48f-1e0a9d3b53f7)

![image](https://github.com/user-attachments/assets/ec450fe8-d328-49a5-bac1-8aba601d1b37)

![image](https://github.com/user-attachments/assets/1df48003-1552-4568-8bf5-2491698fd053)

![image](https://github.com/user-attachments/assets/676db50f-bfb0-48d6-a64e-7c2dd2edd6da)

![image](https://github.com/user-attachments/assets/c8718953-c8e0-4d1a-8bed-21ee5e0d7fea)

**Ground Slash Script**
```C#
void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 180, 0);

        if(GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(SlowDown());
        }
        else
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        if(!stopped)
        {
            RaycastHit hit;
            Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Debug.DrawRay(distance, transform.TransformDirection(-Vector3.up * detectingDistance), Color.red);
            if(Physics.Raycast(distance, transform.TransformDirection(-Vector3.up * detectingDistance), out hit, detectingDistance))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else{
                transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            }

            Destroy(gameObject, destroyDelay);
        }
    }
```

### Ground Slam

This effect involves three stages. The first is a small feedback for the ground slam jump, generating a shockwave with particles and smoke to show the jump’s impact on the surroundings.

![image](https://github.com/user-attachments/assets/fbb0d0be-400a-491b-b13b-b4e00d32a692)

The second part is a Visual Graph that leaves a trail of red loops following a transform.

![image](https://github.com/user-attachments/assets/fd995874-42a3-476c-8e2c-9adc954a1d38)

![image](https://github.com/user-attachments/assets/75c58d8f-ca33-49c6-9259-cbef684bc156)

![image](https://github.com/user-attachments/assets/d312b68f-bc14-4bd9-a860-237db9caeca7)

Finally, the slam effect itself combines multiple effects: three crack sprites and ground effects, two particle effects for brightness with static particles that appear and disappear, two emissive lava splash effects, a debris particle effect for flying rocks, a smoke particle effect, a custom mesh particle effect that makes it look like the "earth" is rising for extra impact, and an animated sphere shader creating a shockwave similar to an explosion.

![image](https://github.com/user-attachments/assets/102276a4-948a-48ef-85a8-0cb5f3b81f6f)

![image](https://github.com/user-attachments/assets/5cb6a965-bb24-4e4b-bcb5-f3dd858a5ff8)

![image](https://github.com/user-attachments/assets/b6ad5f7a-d5b7-462f-aefc-1241c525ae5d)

![image](https://github.com/user-attachments/assets/f23647b5-5a0d-407c-8cf0-c85bc404172d)


### VFX Controller and Cinemachine

To manage and synchronize these effects, I used a script. Although this isn’t the best way to animate VFX (ideally, I would have used a director with animations), I employed tricks such as changing effect layers to make them invisible with timed delays, and coroutines that activate and deactivate game objects with set delays. Additionally, I used Cinemachine to make perspective and camera changes easier, adding shake effects to some effects to enhance their impact.

**VFX Controller**
```C#
void Update()
    {
        smashGroundPS.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        invocationPS.transform.position = invocationPoint.position;
        preInvocationPS.transform.position = invocationPoint.position;

        if (animator != null){
            if (Input.GetKeyDown(KeyCode.Alpha1) && !poweringUp && !jumpAttacking && !groundSlashing)
            {
                PowerUp();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && !jumpAttacking && !groundSlashing && !poweringUp) 
            {
                JumpAttack();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && !groundSlashing && !jumpAttacking && !poweringUp)
            {
                GroundSlash();
            }
        }
    }

    public void PowerUp(){
        animator.SetTrigger("PowerUp");
        powerUpEffect.Play();

        Invoke("Shake", initialDelayPowerUp);

        StartCoroutine(ChangeLayerWithDelay(electricVFX, initialDelayPowerUp, durationPowerUp));
        StartCoroutine(ActivateParticleSystemWithDelay(electricPS, initialDelayPowerUp));

        poweringUp = true;
        StartCoroutine(ResetBool(poweringUp, 3f));

        StartCoroutine(ResetPos(3f));
    }

    public void JumpAttack(){
        animator.SetTrigger("JumpAttack");
        LightningTrailVFX.Play();

        Invoke("Shake", smashGroundDelay);

        StartCoroutine(ActivateParticleSystemWithDelay(jumpShockWavePS, 0.6f));
        StartCoroutine(ActivateWithDelay(FireballShockWaveVFX, smashGroundDelay, 0.7f));
        StartCoroutine(ActivateParticleSystemWithDelay(smashGroundPS, smashGroundDelay));

        jumpAttacking = true;
        StartCoroutine(ResetBool(jumpAttacking, 4f));


        StartCoroutine(ResetPos(4f));
    }

    public void GroundSlash(){
        animator.SetTrigger("GroundSlash");
        Invoke("InstantiateGroundSlash", 1.2f);
        StartCoroutine(ActivateParticleSystemWithDelay(preInvocationPS, preInvocationDelay));
        StartCoroutine(ActivateParticleSystemWithDelay(invocationPS, invocationDelay));

        groundSlashing = true;
        StartCoroutine(ResetBool(groundSlashing, 3.5f));

        StartCoroutine(ResetPos(3.5f));
    }
```

**Camera Controller**
```C#
void Start()
    {
        button1.onClick.AddListener(() => SwitchCamera(camera1));
        button2.onClick.AddListener(() => SwitchCamera(camera2));
        button3.onClick.AddListener(() => SwitchCamera(camera3));
    }

    void SwitchCamera(CinemachineVirtualCamera activeCamera)
    {
        camera1.gameObject.SetActive(camera1 == activeCamera);
        camera2.gameObject.SetActive(camera2 == activeCamera);
        camera3.gameObject.SetActive(camera3 == activeCamera);
    }
```
 
## Group Exercise 4 (Solo Effort Too ._.)
### Shader Exploration

[Shader Project Repository](https://github.com/user-attachments/assets/b90277cf-d295-470f-9745-71a27163c417)

### Displacement
For this exercise, I used the displacement technique we learned while creating the flag shader, but extended it to function across both axes.

![Displacement Shader](https://github.com/user-attachments/assets/458ad444-ec62-4c45-a243-8e650d147fa6)

### Screen Space Refractions
This section leverages the knowledge we gained from the Force Field exercise, particularly focusing on normal maps and screen space manipulation.

![Screen Space Refractions](https://github.com/user-attachments/assets/0a14d695-90eb-472f-930d-8226dc1f022c)
![Another Example](https://github.com/user-attachments/assets/6a56746e-0096-4b33-88c5-681decacf532)

### Cubemap Reflections
This technique is based on the reflections used in the Mario Kart power-up shader.

![Cubemap Reflections](https://github.com/user-attachments/assets/be7b3cd3-40ba-48d8-8a3c-ed98ef7f7dc9)

### Depth-Based Fog
To achieve this effect, I referenced an online tutorial that explained how to use screen depth to create a `Fog` effect with a `lerp` function.

![Depth Fog Shader](https://github.com/user-attachments/assets/dfe10828-6321-45bf-b1cf-ad4e471d6495)
![Example of Depth-Based Fog](https://github.com/user-attachments/assets/92b3d25c-0fff-4cd9-bd35-7525bdeb55df)

### Foam at Intersections and Peaks & Valleys
For this, I used two modules: one to detect intersections using the depth subgraph, and another to generate foam at the peaks and valleys of the waves by converting a normal map into noise.

![Foam Shader](https://github.com/user-attachments/assets/919cf887-4550-4eb6-aa57-e55850e84ed6)
![Peaks and Valleys](https://github.com/user-attachments/assets/b96ca262-763c-4980-ab5d-bb1617d4f5f2)
![Wave Effect](https://github.com/user-attachments/assets/76a2d2a0-176b-4fa8-be3b-105c66058a8e)

### Blinn-Phong Lighting with External Light Support
Here, I combined the Blinn-Phong shader, which we used for skin rendering in class, with external light support as seen in the ToonLit shader. The result separates the specular highlights and the gradient of soft and hard light colors, while also adding support for multiple light sources in the scene.

![Blinn-Phong Lighting](https://github.com/user-attachments/assets/544c028c-3193-43f7-8119-0cb655833e61)
![Lighting Setup](https://github.com/user-attachments/assets/ed1ea6e2-c313-43bf-bf5c-123feba0fc78)
![image](https://github.com/user-attachments/assets/0fab62fd-4ab7-4e29-b230-a9f1b05e5275)

## Midterm 2 ShaderGraphs

### Distortion
I built this effect based on the ForceField exercise, using the principles we learned there to replicate the distortion.

![Distortion Shader](https://github.com/user-attachments/assets/14ee8677-50de-4703-b025-d30749060eff)
![Distortion Example](https://github.com/user-attachments/assets/71fdd6ff-0262-44a2-98bc-5f4fd296fd26)

### Smoke Movement
This exercise was inspired by Group Exercise 2. To create the smoke effect, I used a transparent mask in combination with a moving UV to distort the texture.

![Smoke Normals](https://github.com/user-attachments/assets/d4cff06c-5f55-4551-870e-753ea28e7106)
![Smoke Example](https://github.com/user-attachments/assets/d1202c69-c306-4bd1-ae4d-12abdcb873d2)

### Erosion
For this shader, I applied the basic erosion technique learned from the Mario power-up combined with a mask effect.

![Erosion Shader](https://github.com/user-attachments/assets/fab05954-229f-45ae-a9fb-105668cb0b07)
![Erosion in Action](https://github.com/user-attachments/assets/44f6e99f-8313-4d37-ad52-d1cf929f4b5c)

### Texture Dissolve
This shader is an extension of the erosion technique, with slight modifications to the parameters.

![Dissolve Texture](https://github.com/user-attachments/assets/8ed4a566-8343-41f6-9aab-5a14098a89f1)
![Texture Dissolve Example](https://github.com/user-attachments/assets/87d9d881-b286-4c42-9930-a76758b34a59)

## Group Exercise 2 _(Solo Effort 😅)_

![Black Hole Effect](https://github.com/user-attachments/assets/1928519f-675b-4b32-8acf-c68bb360dda9)

### Anticipation (Black Hole)
For this effect, I wanted to replicate the look of a black hole. I imagined particles being pulled into a bright vortex, so I focused on creating that gravitational pull effect.

![Black Hole Vortex](https://github.com/user-attachments/assets/9428d148-1ec3-47e8-897f-5b51ff8e0823)

#### Event Horizon:
This is the simplest particle effect, where I spawn the sprite, change its color and brightness using HDR, and adjust other parameters like delay and size curves to fit the overall look.

![Event Horizon Example](https://github.com/user-attachments/assets/dd624785-83cb-46ad-ba5b-94b216b252c1)

#### Orbital Particles:
These particles are randomly positioned on the surface of a sphere. In the update phase, several things happen: I use a set position to manage their angular velocity, a `conform to sphere` function to ensure they follow an orbital path, and additional adjustments to control their movement into a disk shape around the black hole.

![Orbital Particles](https://github.com/user-attachments/assets/61e40e75-5308-44a0-99c7-fe7a9ebce941)

#### Orbital Particle Trails:
These are spawned from a GPU event, inheriting the properties of the orbital particles like position, color, and size. The rest of the variables are purely aesthetic, controlling color, lifespan, and more.

![Particle Trails](https://github.com/user-attachments/assets/03355f16-e36f-47f5-a1a1-615b560de0b0)

#### Star Dust / Smoke:
I aimed to replicate the "smoke" rising around the black hole, giving the final stage of dissipation a windy, dispersing effect. These particles also orbit the sphere, but with unique properties like linear drag for friction and a `flipbook` module for animating the smoke texture.

![Stellar Smoke](https://github.com/user-attachments/assets/2ef9938d-bcd1-4678-b809-0d10e8375582)
![Another Smoke Example](https://github.com/user-attachments/assets/04def803-0ede-455b-b0c4-efc58d5e4955)

#### Distortion:
I applied a shader graph to add distortion to the space around the black hole, rotating UVs and applying a twirl effect. This, combined with dithering and masking, created a dynamic distortion effect.

![Distortion Effect](https://github.com/user-attachments/assets/af3e883c-3607-437f-9185-1fb49bb22dec)
![Distortion Example](https://github.com/user-attachments/assets/68269b42-470e-4373-9f4c-251c2d5f0618)

Finally, I used the visual effect graph to animate the entire effect, controlling delay, scale, position, and even the alpha channel of the mesh for smooth transitions.

![Visual Effect Graph](https://github.com/user-attachments/assets/6437b80c-303c-485e-b51b-e8ca20744917)

### Explosion
This phase simulates the final collapse and explosion, similar to a star's death. All particles converge inward before bursting outward.

![Explosion Effect](https://github.com/user-attachments/assets/13a842f5-651c-43dc-b3f0-36cd2bcd8bcd)
![Explosion Example](https://github.com/user-attachments/assets/dc6117a7-d32a-4e99-8a14-ae4b900abd17)

#### Particle Explosion:
The orbital particles and smoke are pulled toward the center before disappearing, while a new set of particles are expelled outward, rapidly scaling with velocity. 

![image](https://github.com/user-attachments/assets/f93b7eb3-50f3-4c2f-a92b-cc3ed140548e)


**Electricity:**
To replicate the electricity shown in the drawing, I created particles that appear right at the moment of the explosion. These particles consist of "heads" with trails that follow them, similar to orbital effects. The main difference is that both the heads and trails have turbulence applied to them, which introduces noise into their movement, making it more erratic and resembling lightning.

![image](https://github.com/user-attachments/assets/6f1e3725-90e8-4ecc-9167-1c322024203e)

### Final Explosion & Dissipation

In the final phase of the black hole effect, the particles explode outwards, mimicking a supernova or a black hole collapse, with a dissipating effect that transitions the scene into stillness.

#### Particle Dissipation:
After the main explosion, a series of particles, representing cosmic dust and light remnants, are dispersed in all directions. This is achieved by modulating the particle system to emit from a spherical point of origin, with varying velocity and drag to create a realistic dissipation effect.

![image](https://github.com/user-attachments/assets/9ca05937-107d-4948-98eb-244adc616d9a)

![image](https://github.com/user-attachments/assets/9957bd3b-4841-41fa-ac60-a1aca447f402)

### Animation
The visual effect graph here also controls the fade-out of the particles, reducing their opacity over time and blending into the background seamlessly, as if they’re fading out of existence.
And last, to complement the collapse, a subtle light emission is triggered from the center of the explosion. This emission grows in intensity just before it fades away, simulating the final energy release of the collapsing black hole. 

![image](https://github.com/user-attachments/assets/d739d7e1-183b-4b39-9823-6bdc0caf691b)

![image](https://github.com/user-attachments/assets/d99d272e-9e2c-43de-a451-61d55a4d5de4)

## Midterm 1 Tornado

### Creation of Static Light
For the creation of the first effect, I decided to start with the static light on the ground, which changes its size according to the particle's lifetime. It's a very simple particle, where only a few values such as duration, particle count, emission, and size are modified to create this effect.

![image](https://github.com/user-attachments/assets/4e40bbf2-cbf5-4ed1-8ea8-b908664a8b77)

### Creation of Floor Waves
The second effect I chose to create was the floor waves, as it’s a particle very similar to the previous one. It simply changes its color and transparency over time, and its size starts at zero and grows until it disappears.

![image](https://github.com/user-attachments/assets/ebfd666b-818f-491b-ad04-45fe946c1ad7)

### Creation of Upward Dots
This was the third effect I decided to tackle, as it was a simpler version of the trails effect and served as a base. I simply modified the cone's emission and adjusted the orbital velocity and size to achieve small floating dots.

![image](https://github.com/user-attachments/assets/7fd2d0d6-ade8-4091-9371-d393424618ae)

### Creation of Upward Trails
Similarly, I continued with this effect due to the ease of using the base from 'Upward Dots,' as there was no need to drastically modify the values to achieve the trails.

![image](https://github.com/user-attachments/assets/b2978a4b-5bf0-4092-a468-13b054543c06)

### Creation of Air Cone (After Class)
This is the small cone that flattens over time and more cones appear. I didn’t finish this effect during class, but I decided to upload it later between 8:50 and 9:00 a.m. For this, I did something similar to the floor waves, but this time I had to export a cone from Blender as a custom mesh and modify the size of its axes separately over time.

![image](https://github.com/user-attachments/assets/639b3be3-2954-4934-8761-b17904e4ea79)

### Creation of Outer Tornado
Last but not least, I decided to create the tornado, which, according to my evaluation, required a custom mesh and rotation over lifetime. I also applied emissions similar to the static particle. It was simply a matter of adjusting the visual effects and its size.

![image](https://github.com/user-attachments/assets/96cdedb2-9994-41e2-ac40-ec552a04f1f0)

### Creation of Inner Tornado
I used the same tornado but with a material of a different color, altered the transparency of the effect, and individually rescaled each axis in Unity to make it thinner.

![image](https://github.com/user-attachments/assets/c73358ab-1f6e-4514-bd58-fce948c1e300)

### Final Result

![GifTornado](https://github.com/user-attachments/assets/b212bc4b-d6f6-471a-8759-24f6d406574d)

### Final Result With Adjustments After Class
Here, I included the creation of the air cone and adjusted the values of the stripes to better fit the tornado, making it visually more appealing.

![GifTornado_PostAjustes](https://github.com/user-attachments/assets/97fed185-0ecd-4db7-b0d9-8b742c282d4c)

## Evidence of Used Assets
### Materials
- Bluestatic (Used for the static particle, floating dots, and trails).
- Tornado and Tornado Air.
- Waves (For the FloorWaves effect).

![image](https://github.com/user-attachments/assets/ca5da16e-d53e-4e8e-9743-103c9f50cf44)

### Textures
- CircleTexture (Used in the Healing VFX class).
- Tornado and Tornado 1 (Duplicated for texture experimentation).
- White Particle (A texture I experimented with but didn’t fit the final design).

![image](https://github.com/user-attachments/assets/385402ee-cd16-447d-8a3f-c49c8b32811d)

### Models
- Tornado 1 and Tornado 2 (Duplicated for UV experimentation).
- Cone (Attempt at creating the effect of a flattening cone, but I didn't finish it in class).

![image](https://github.com/user-attachments/assets/364a3708-957c-441f-9eb2-05b8f2509bd2)






