VAR speaker= "Friend"
VAR order1="strawberry smoothie"
VAR order2="banana smoothie"
VAR knowledge1=0
-> Convo0

=== Convo0 ====
= C0Stitch1
Hey player_name! It is I, your best friend. How do you feel about your first day working at a smoothie store?
    + [I'm nervous...]
        ~speaker="You"
        -> C0Stitch2
     + [I'm excited!]
        ~speaker="You"
        -> C0Stitch3
    + [Padding]
        -> END

= C0Stitch2
I'm actually pretty nervous. What if I screw up orders?
    + [Next]
        ~speaker="Friend"
        -> C0Stitch4
     + [Padding]
        -> END
    + [Padding]
        -> END
= C0Stitch3
I was born ready! Bring it on! 
    + [Next]
        ~speaker="Friend"
        -> C0Stitch5
     + [Padding]
        -> END
    + [Padding]
        -> END
        
= C0Stitch4
Relax. People around here are really nice. Here comes your first customer!
    + [Ok]
        -> END
     + [Padding]
        -> END
    + [Padding]
        -> END
= C0Stitch5
That's the spirit! Here comes your first customer. 
    + [Ok]
        -> END
     + [Padding]
        -> END
    + [Padding]
        -> END

=== Convo1 ====
= C1Stitch1
Hi, I'd like a {order1}
    + [Give right thing]
        -> C1S1answer1
     + [Give wrong thing]
        -> C1S1answer2
    + [Padding]
        -> END
= C1S1answer1
Thanks! This is just what I needed, I've had a lot on my mind lately.
    + [Tell me more]
        ~speaker="You"
        -> C1Stitch2
     + [Not interested]
        ~speaker="You"
        -> C1Stitch3
    + [Padding]
        -> END

= C1S1answer2
Hey... this isn't what I ordered.
    + [Next]
        ~speaker="Customer2"
        -> END
    + [Padding]
        -> END
    + [Padding]
        -> END

= C1Stitch2
Hey, I got the time. What's troubling you?
    + [Next]
        ~knowledge1=1
        ~speaker="Customer1"
        -> C1Stitch2P2
    + [Padding]
        -> END
    + [Padding]
        -> END
= C1Stitch2P2
Well, I got so many pants I do not know what to do with them.
    + [Ok]
        ~speaker="You"
        -> C1Stitch2P3
     + [Padding]
        -> END
    + [Padding]
        -> END

= C1Stitch2P3
Ok, I will keep that in mind.
    + [Next]
        ~speaker="Customer2"
        -> END
     + [Padding]
        -> END
    + [Padding]
        -> END
        
= C1Stitch3
Oh...ok. I'm kind of busy and there's another customer coming.
    + [Next]
        ~speaker="Customer2"
        -> END
     + [Padding]
        -> END
    + [Padding]
        -> END
        
=== Convo2 ====
= C2Stitch1
Hi, I'd like a {order2}
    + [Give right thing]
        -> C2S1answer1
     + [Give wrong thing]
        -> C2S1answer2
    + [Padding]
        -> END
= C2S1answer1
Yum! I really needed this- this week's been rough. 
    + [Tell me more]
        ~speaker="You"
        -> C2Stitch2
    + [Padding]
        -> END
    + [Padding]
        -> END

= C2S1answer2
Hey... this isn't what I ordered.
    + [Next]
        ~speaker="Narrator"
        -> Lose2
    + [Padding]
        -> END
    + [Padding]
        -> END

= C2Stitch2
What's the matter? 
    + [Next]
        ~speaker="Customer2"
        -> C2Stitch2P2
    + [Padding]
        -> END
    + [Padding]
        -> END
= C2Stitch2P2
I do not have any pants. I would love some pants.
    + [Ok]
        ~speaker="You"
        -> C2Stitch3
     + [Padding]
        -> END
    + [Padding]
        -> END
= C2Stitch3
{knowledge1>0:->Win}
{knowledge1<1:->Lose}
    + [Padding]
        -> END
    + [Padding]
        -> END
    + [Padding]
        -> END

=== Win ====
= WStitch1
Hey... I actually know someone with too many pants! Do you know Customer1?
    + [Next]
    ~speaker="Narrator"
        -> WStitch2
    + [Padding]
        -> END
    + [Padding]
        -> END
= WStitch2
You managed to bring two people in the community closer together.
    + [Next]
        ->END
    + [Padding]
        -> END
    + [Padding]
        -> END
=== Lose ====
= LStitch1
I hear ya, but I'm not sure how to help with that.
    + [Next]
    ~speaker="Narrator"
        -> LStitch2
    + [Padding]
        -> END
    + [Padding]
        -> END
= LStitch2
You failed to help your community.
    + [Next]
        ->END
    + [Padding]
        -> END
    + [Padding]
        -> END
=== Lose2 ====
= L2Stitch1
You failed to help your community.
    + [Next]
        ->END
    + [Padding]
        -> END
    + [Padding]
        -> END
-> END