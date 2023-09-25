INCLUDE Globals.ink

Mat: My master, what can I help you?
{matState:
-1: ->Talk01
-2: ->Talk02
-3: ->Talk03
-else: ->DONE
}

===Talk01===
Mat: Good day, my master. You should stay stasis now. We are still on the way to the planet AST-28.
You: Did you realize the shake? Did our spaceship crash?
Mat: Based on the flight recorder, it didn't crash on anything.
You: Weird. A shake just woke me up. Can you find out what's happened?
Mat: Yes, my master. Besides, it is reported that the energy supply of the spaceship is unstable. Could you help to check any breakdown of the circuit breaker?
You: But I don't know how to do.
Mat: My master, you're the cleverest girl I have even seen. I believe you can fix the problem.
Mat: Just don't forget to take the screwdriver. The circuit breaker is at the corridor.
You: Okay. I'll go for it.
~setItem="Screwdriver"
~isScrewdriverOn=true
~matState=0
->DONE

===Talk02===
Mat: My master, did you fix the broken device?
You: Yes but it looks like being punched by someone intentionally.
Mat: No worry. Now everything is alright. My master, it’s time to take a rest.
+[There should be some problems.] You: You should let me know all about the trip and the spaceship.
  Mat: My master, for the reasons of flight security and commercial secret, I cannot tell you everything. Please trust me. I won’t let you get in danger.
      ++[You look at Mat with distrust.] I should go to find out the truth of that.
      ~matState=0
      ~isComputerOn=true
      ->DONE
      ++[Alright. I'd trust you.] I'm going to sleep now. Wake me up when we arrive.
      ~isSleepAgain=true
      ->DONE
+[I'm going to sleep now.] Wake me up when we arrive.
~isSleepAgain=true
->DONE

===Talk03===
Mat: My master, You should stay at your room and go back to sleep.
You: Don’t play me a fool. I know what you are doing to me, smuggler.
Mat: My master, You shouldn't read the document in the computer - it's commercial secret. 
Mat: Now you’ve violated the rule of the trip. I've to lock you down until we arrive the planet.
*[Hit Mat with the hammer]->Hit

===Hit===
~setItem=""
~isMatHit=true
Mat: My mas^%&$t(*)er, the passen&*^*&ger in theeeeee spaceship&*%$ shall not use vi&*_(*olence. I have to loc*&^&$k you...
Mat is broken down. You stop the spaceship and play a S.O.S message to any passing spaceships.
There is a cargo spaceship not so far which has received your message. You're now on the way home.
Yet, not every victims are that lucky.
~isGameEnd=true
->DONE