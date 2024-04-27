-> main

=== main ===
Welcome to our virtual exhibition, I'm Inanna, your virtual curator. I'm thrilled to guide you through this immersive journey into art and innovation.
-> main2

=== main2 ===
How can I help you?
+ [Can you help me orient this exhibition?]
    -> map
+ [I want to know more about the background of this exhibtion?]
    -> intro
+ [I am good for now. Thank you!]
    Of course! Feel free to reach out whenever you need help!
    -> END
-> main2

=== map ===
This exhibition consists of two main exhibtion halls separated by a division wall. On the east side is hall 1 and on the westside is hall 2. 
You can use the controller to teleport to wherever you want. Simply point to a location then hit the grip button.
-> map2

=== map2 ===
Do you need any further help?
+ [Do you have a map of this exhibition?]
    Sure thing! Here is a map.
    -> END
+ [Thank you. (back to main dialogue)]
    -> main2
-> main2

=== intro ===
This virtual sculpture exhibition is titled "Forms of Thought - where the timeless and the modern converge". 
This collection features seminal works including Rodin's introspective masterpiece "The Thinker," the iconic grace of "Venus de Milo," and the majestic "Head of Helios," offering a unique exploration of human expression through form. 
As you navigate through this digital space, each sculpture invites you to ponder the historical and aesthetic significance of these celebrated creations, providing a profound sensory and intellectual experience.
-> intro2

=== intro2 ===
Do you need any further help?
+ [Can you give me some highlights of this exhibtion?]
    Absolutely. At the center of the east hall is the Venus statue. Behind it by the wall is 'Head of Helios'. At the middle of the division wall is Rodin's thinker, and at the other end of the east hall is Napoleon. You will find some statues of animals in the west exhibtion hall.
    -> intro
+ [Thank you. (back to main dialogue)]
    -> main2
->main2
    
    