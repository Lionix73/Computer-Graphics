# ComputacionGrafica

## Ejercicio Grupal 2 _(Me hice solo xd)_

![EfectoBlackHole](https://github.com/user-attachments/assets/1928519f-675b-4b32-8acf-c68bb360dda9)

### Anticipacion (Agujero Negro)
La primera parte del efecto segun mi perspectiva del dibujo era muy parecido a un agujero negro, porque parecia atraer las particulas al vortice y un origen muy brillante entonces me centre en tratar de realizar un agujero negro.

![image](https://github.com/user-attachments/assets/9428d148-1ec3-47e8-897f-5b51ff8e0823)

**Agujero Negro/Evento de Horizontes:**
Es la particula mas sencilla, esta simplemente consiste de instanciar el sprite, cambiar el color y brillo con HDR, y modificar los demas parametros como el delay o curva de tamaño para que coordine con el efecto.

![image](https://github.com/user-attachments/assets/dd624785-83cb-46ad-ba5b-94b216b252c1)

**Particulas Orbitales:**
Estas son una instancia de varias particulas, donde su posicion inicial es aleatoria al rededor de una superficie de una esfera, despues en el update ocurren varias cosas, primero un set position que ayuda a que las particulas no se descontroler con su velocidad angular, un conform to sphere para que puedan seguir una trayectoria orbital, donde entre mas avanza la vida de la particula mas fuerza tiene hacia el centro, otro set position para controlar que todas las particulas aparezcan solo en una franja de la esfera y asi formen un disco alrededor del agujero negro y por ultimo un evento a lo largo del tiempo para generar trails de las particulas.

![image](https://github.com/user-attachments/assets/61e40e75-5308-44a0-99c7-fe7a9ebce941)

**Trails Particulas Orbitales:**
Estas son iniciadas por un GPU event y heredan de las particulas, la posicion, color y tamaño. De resto todas sus variables son simplemente esteticas para controlar su color, vida, etc.

![image](https://github.com/user-attachments/assets/03355f16-e36f-47f5-a1a1-615b560de0b0)

**Humo/Polvo Estelar:**
Queria tratar de replicar este "humo" que se levantaba alrededor del efecto ademas para poder usarlo en la etapa final de disipacion para generar mas este efecto de "viento" que iba llevandose lo que quedaba. Principalmente esto usa un inicio muy parecido a las particulas orbitales y tambien su movimiento al rededor de Conform To Sphere, usando cosas iguales como la fuerza a traves del tiempo, el control de su rotacion o controlar la posicion para que aparezcane en un disco al rededor del centro. Pero tiene algunos modulos distintos como Linear Drag para generar una friccion Flipbook para generar un efecto mas "animado" al sprite del humo. Pero tambien tiene un set velocity que hace parte de la etapa final de `disipacion`, pues cuando ya esta llegando a su etapa final de vida se multiplica su velocidad por un numero grande para generar una explosion. 

![image](https://github.com/user-attachments/assets/2ef9938d-bcd1-4678-b809-0d10e8375582)

![image](https://github.com/user-attachments/assets/04def803-0ede-455b-b0c4-efc58d5e4955)

**Distorsion:**
Para poder aplicar un shader graph queria darle una distorsion al espacio alrededor del agujero negro, entonces aplique un shader graph que consta de una rotacion de las UV, deformadas por un twirl, que despues afectan a la textura que toma como imagen la screen position, y el alpha del efecto es controlada por un Dither y una mascara para poder volver transparente las partes del plano que tengan menos influencia del efecto de distorsion.

![image](https://github.com/user-attachments/assets/af3e883c-3607-437f-9185-1fb49bb22dec)

![image](https://github.com/user-attachments/assets/68269b42-470e-4373-9f4c-251c2d5f0618)

Y finalmente para poder animarlo junto a los demas efectos use el vissual effect graph para poder modificar el delay, escala, posicion, angulo y mas importante de todo el mesh que tiene el material del shader donde puedo modificar el alpha del mesh (lo del Dither y la mascara) por medio de una curva que se ajuste al efecto.

![image](https://github.com/user-attachments/assets/6437b80c-303c-485e-b51b-e8ca20744917)

### Explosion
Esta fase hace parte del momento donde todo empieza a envolverse en su propio centro para terminar explotando con fuerza hacia fuera, pareciendose un poco a la muerte de una estrella fue lo que trate de replicar en esta fase.

![image](https://github.com/user-attachments/assets/13a842f5-651c-43dc-b3f0-36cd2bcd8bcd)

![image](https://github.com/user-attachments/assets/dc6117a7-d32a-4e99-8a14-ae4b900abd17)

**Orbitales, Humo, Origen y Distorsion:**
Todos estos efectos al final de su vida modifican algo para ir hacia el centro y/o desparecer, los orbitales y el humo son atraidos mas fuertemente al origen y los orbitales desaparecen, el origen del agujero negro se vuelve mas pequeño y la distorcion empieza a desvanecerse.

**Explosion de particulas y humo:**
En esta fase ocurren tres cosas, lo primero que ya explique con el humo. Es expulsado con velocidad fuera del origen, y pasan dos efectos nuevos. Uno de explosion de particulas, el cual usa una logica muy parecida a la del humo donde simplemente setiamos su velocidad a una bastante rapida en el inicio, y el origen de su posicion es en una esfera muy pequeña, ademas de que modifico su escala y que se oriente y modifiquen dependiendo de la velocidad para hacerlos mas alargados. Por ultimo tambien tienen un linear drag para que no salgan volando si no que paren un poco despues de la explosion.

![image](https://github.com/user-attachments/assets/f93b7eb3-50f3-4c2f-a92b-cc3ed140548e)


**Electricidad:**
Para tratar de replicar la electricidad que se muestra en el dibujo tambien genere unas particulas que aparecieran justo en la explosion y que se basan en tambien tener unas particulas como "cabeza" y un trail las cuales siguen a estas cabezas, es algo parecido al de los orbitales, lo unico que cambia es que los dos tienen turbulencia, que es una forma de generar ruido en el movimiento de estas particulas para que sea mas erratico y parecia un relampago.

![image](https://github.com/user-attachments/assets/6f1e3725-90e8-4ecc-9167-1c322024203e)

### Disipacion
Por ultimo en la finalizacion del efecto habia que dar una ilusion de que el efecto terminaba con un desvanecimiento del polvo o de lo que fue alguna vez antes de explotar. Esto trate de conseguir principalmente con el final de la explosion de particulas y tambien agregando humo, los cuales mueren lentamente y generan la sensacion de desvanecimiento que transimte el dibujo original (solo que menos epico).

![image](https://github.com/user-attachments/assets/9ca05937-107d-4948-98eb-244adc616d9a)

![image](https://github.com/user-attachments/assets/9957bd3b-4841-41fa-ac60-a1aca447f402)

### Animacion
Por ultimo para que todo cumpliera con lo especificado, cogi una animacion para tratar de simular el POV del mago que genera este hechizo de agujero negro y que se sincronizara por medio de la timeline del Visual Effect Graph y la Animacion de Mixamo. Ademas por temas de usabilidad puse un pequeño boton en la esquina para repetir el efecto sin necesidad de salir de play.

![image](https://github.com/user-attachments/assets/d739d7e1-183b-4b39-9823-6bdc0caf691b)

![image](https://github.com/user-attachments/assets/d99d272e-9e2c-43de-a451-61d55a4d5de4)

## Parcial 1 Tornado
### Creacion de Static Light
Para la creacion del primer efecto decidi empezar por la luz estatica en el suelo la cual cambiaba su tamaño con el tiempo de vida de la particula. Es una particula muy simple, donde solo algunos valores de duracion, cantidad de particulas, emision y tamaño es modificado para crear este efecto.

![image](https://github.com/user-attachments/assets/4e40bbf2-cbf5-4ed1-8ea8-b908664a8b77)

### Creacion Floor Waves
El segundo efecto que decidi crear fueron los floor waves, debido a que es una particula muy parecida a la anterior, donde simplemente cambio su color y transparencia con el tiempo, ademas de que su tamaño empiez en 0 y vaya creciendo hasta desaparecer.

![image](https://github.com/user-attachments/assets/ebfd666b-818f-491b-ad04-45fe946c1ad7)

### Creacion Upward Dots
Este fue el tercer efecto que decidi abordar pues era una version mas facil del efecto de trails, y me servia como base, en el cual simplemente era modificar la emision del cono. Y su velocidad orbital y tamaño para lograr que fueran pequeños dots flotantes.

![image](https://github.com/user-attachments/assets/7fd2d0d6-ade8-4091-9371-d393424618ae)

### Creacion Upward Trails
Al igual decidi seguir con este por la facilidad de usar la base anterior de los 'Upward Dots' pues no habia necesidad de modificar de forma extrema los valores para lograr los trails. 

![image](https://github.com/user-attachments/assets/b2978a4b-5bf0-4092-a468-13b054543c06)

### Creacion Cono de aire (Despues de clase)
Es el pequeño cono que se va aplastando y van saliendo mas, este efecto no alcance a montarlo en clase pero aun asi despues de salir decidi subirlo, entre las 8:50 y 9:00 am. Para este hice algo parecido al floor waves pero esta vez tuve que exportar un cono del blender para que sea una mesh personalizada y ademas cambiaba el tamaño de sus ejes separados en el tiempo.

![image](https://github.com/user-attachments/assets/639b3be3-2954-4934-8761-b17904e4ea79)

### Creacion del Tornado Exterior
Y por ultimo pero no menos importante decidi realizar el tornado, el cual requeria segun mi valoracion, un mesh personalizado, y generar una rotacion overlifetime. Al igual que aplicar emisiones parecidas 
a la particula estatica. Simplemente era cuestion de ajustar los efectos visuales y su tamaño.

![image](https://github.com/user-attachments/assets/96cdedb2-9994-41e2-ac40-ec552a04f1f0)

### Creacion del Tornado Interior
Use literalmente el mismo tornado pero con un material con un color diferente, cambie la transparencia del efecto y lo reescale individualmente cada eje en el unity para que fuera mas delgado.

![image](https://github.com/user-attachments/assets/c73358ab-1f6e-4514-bd58-fce948c1e300)

### Resultado Final

![GifTornado](https://github.com/user-attachments/assets/b212bc4b-d6f6-471a-8759-24f6d406574d)

### Resultado Final Con Ajustes despues de Clase
Aqui esta incluida la creacion del cono de aire y tambien ajuste los valores de los stripes para que se ajustaran mejor al tornado y fuera mas bonito visualmente.

![GifTornad_PostAjustes](https://github.com/user-attachments/assets/97fed185-0ecd-4db7-b0d9-8b742c282d4c)

## Evidencia de Assets usados
### Materiales
- Bluestatic (Usado para la particula estatica, floating dots y trails).
- Tornado y Tornado Air.
- Waves (Para el efecto de FloorWaves)

![image](https://github.com/user-attachments/assets/ca5da16e-d53e-4e8e-9743-103c9f50cf44)

### Texturas
- TexturaCirculo (Usada de la clase de Healing VFX).
- Tornado y Tornado 1 (Duplicado por experimentacion de texutras).
- White Particle (Una textura que experimente pero no encajo en el diseño final).

![image](https://github.com/user-attachments/assets/385402ee-cd16-447d-8a3f-c49c8b32811d)

### Modelos 
- Tornado 1 y Tornado 2 (Duplicado por experimentacion de UV's).
- Cono (Intento de realizacion del efecto que es un cono que se aplana, pero no alcance en clase).

![image](https://github.com/user-attachments/assets/364a3708-957c-441f-9eb2-05b8f2509bd2)








