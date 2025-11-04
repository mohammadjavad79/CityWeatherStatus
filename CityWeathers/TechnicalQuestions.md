# Technical Questions

1. ### How much time did you spend on this task ? 

    It took around 10 hours in total.
    The main reason it took that long was that I spent a lot of time working on creating database tables, storing data in them, and implementing logging using Serilog.
    I had previously worked with the Laravel stack and the PHP language, which I’m quite proficient in. Since this project used a new stack for me, it naturally took a bit more time to get comfortable with it.
    For this project, I used an SQLite database to store city data (by default, cities of Iran), as well as to save requests and responses in JSON format. I also stored the processed city data that might be used later for reporting or analytics purposes.
    In addition, I implemented exception handling and a global exception middleware to manage errors more effectively.



2. ### What is the most useful feature recently added to your favorite programming language?

    In .NET, the addition of the AOT compiler was really interesting to me. I also find the JIT compiler fascinating. The removal of the Program class requirement, allowing for a more functional programming style, is something I really like.
    Another feature that stands out to me is how C# supports asynchronous programming, which is something PHP lacks.
    As for PHP, the introduction of enum types was a great addition, and even before that, type hinting made the language feel more structured.
    Overall, I find most modern programming language features amazing, but .NET in particular is very impressive and attractive to me for many reasons.

3. ### how do you identity diagnose a performance issue in a production environment?

    In a production environment, we can identify performance issues using monitoring tools like **Sentry** or **Grafana**.  
    With **Sentry**, for example, we can track **N+1 queries**, **slow queries**, or an increased number of **exceptions**.  
    If we define a custom exception type for each kind of error, we can easily filter and analyze them.

    After identifying the problematic endpoint or section of code, we review it to understand the root cause —  
    whether it’s due to an inefficient query, a poorly written query, an infinite loop, or another performance bottleneck.

4. ### what is the last technical book you read or technical conference you attended?

    #### ASP.NET Core and .NET
    
    I read the first few pages of **“ASP.NET Core in Action” by Andrew Lock** and also watched several learning videos about **.NET**.
    From these, I learned how **.NET** works — especially its **startup process**, the **middleware pipeline**, and some aspects of **.NET syntax**.
    
    #### SQL and Indexing
    
    I also studied **SQL indexing** and how they index columns under the hood, including what makes a column a good candidate for indexing (for example, **cardinality**), as well as **composite indexes**, how their filtering order affects performance, and how **JSON index fields** work.

    #### Software Engineering Concepts
    
    Additionally, I watched a **conference on software engineering**, which discussed which types of tasks are best suited for software engineers and how responsibilities should be structured. 

    In my opinion, a **software engineer** should be able to take ownership of a task, analyze it, and then break it down into smaller sub-tasks independently.


5. ### what is your opinion about this technical test?
    
    this test can be good to find out what developers and software engineers will think about task and how they will thought

6. ### About Me

    A passionate **Software Engineer** with a strong interest in **.NET**, **C#**, and modern backend technologies.  
    Originally experienced in **PHP**, I transitioned into the **.NET ecosystem**, where I’ve worked both **self-taught** and **professionally** in my current company.
    
    I have hands-on experience in **financial systems**, **marketplaces**, **accounting software**, and **banking platforms**, including integrating with **bank APIs** and **payment gateways**.
    
    Driven by curiosity and continuous learning, I’m highly motivated to deepen my expertise in **backend development**, **database design**, and **system architecture**.  
    I thrive on solving complex problems, optimizing performance, and collaborating within team environments.  
    I’m also interested in **data science**, **system design**, and leveraging **modern monitoring tools** to build scalable, reliable systems.