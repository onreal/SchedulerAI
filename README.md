# SchedulerAI API

SchedulerAI is a developer-friendly API that streamlines scheduled messaging with built-in AI-powered content generation. 
Effortlessly combine scheduling logic, AI creativity, and multi-channel notifications into your product in minutes.

With the generative API you can translate any user prompt to a unified JSON object.

---

## 🚀 Key Features

- **🕒 Scheduled Messaging**  
  Schedule messages to be sent at specific times or recurring intervals with full control over timing and delivery logic.

- **🤖 AI-Powered Content Generation**  
  Generate dynamic, context-aware message content using OpenAI models (ChatGPT). Customize tone, style, and content type.

- **📱 Multi-Channel Delivery**  
  Deliver messages via SMS (Twilio), with planned support for email, push notifications, and chat apps (Telegram, WhatsApp).

- **👥 Recipient Management**  
  Manage recipients effortlessly with grouping, segmentation, and tagging.

- **📅 Integration-Friendly**  
  Fetch and sync events from Google Calendar and other platforms (coming soon), enabling smart scheduling based on user calendars.

- **🔐 Secure & Multi-Tenant Ready**  
  Built with tenant isolation and secure access patterns. Perfect for SaaS platforms and multi-organization use cases.

---

## ⚡ Quick Start

1. **Sign Up & Get API Key** via RapidAPI.
2. **Connect Integrations** (Twilio, OpenAI).
3. **Create a Schedule** using `/api/schedules`.
4. **Relax** — SchedulerAI will handle the rest!

---
## ⚡🕒 Docker Install
Clone this repository locally, and on the root of the solution:
`docker-compose up --build`
Enjoy :)

---

## 💡 Example Use Cases

- Automated appointment reminders with personalized AI messages.
- Daily motivational quotes or health tips via SMS.
- AI-generated marketing campaign sequences scheduled for specific time windows.
- Smart notifications based on calendar events.
- Generate events from user prompts and return a unified JSON to use within your application.
---

## 🤖 Generative reminders
`POST /v1/reminders/generate`

### Example 1

```
REQUEST
{
"message": "Tomorrow meeting with Mike and John at 14:00 at office, remind me at 13:30, send them and Anna an email after."
}
```

```
RESPONSE
[
  {
    "names": ["Mike", "John", "Anna"],
    "eventDateTime": "2025-07-19T14:00:00",
    "location": "office",
    "remindBefore": "2025-07-19T13:30:00",
    "actions": [
      {
        "actionType": "send_email",
        "targetNames": ["Mike", "John", "Anna"],
        "note": "Hello Mike, John, and Anna, just a reminder that we have a meeting today at 14:00 in the office."
      }
    ]
  }
]
```

### Example 2

```
REQUEST
{
"message": "Tomorrow meeting with Mike, John, and Anna at 14:00 at office. Send each of them an email separately."
}
```

```
RESPONSE
[
  {
    "names": ["Mike", "John", "Anna"],
    "eventDateTime": "2025-07-19T14:00:00",
    "location": "office",
    "remindBefore": null,
    "actions": [
      {
        "actionType": "send_email",
        "targetNames": ["Mike"],
        "note": "Hello Mike, just a reminder that we have a meeting today at 14:00 in the office."
      },
      {
        "actionType": "send_email",
        "targetNames": ["John"],
        "note": "Hello John, just a reminder that we have a meeting today at 14:00 in the office."
      },
      {
        "actionType": "send_email",
        "targetNames": ["Anna"],
        "note": "Hello Anna, just a reminder that we have a meeting today at 14:00 in the office."
      }
    ]
  }
]
```

### Example 3

```
REQUEST
{
"message": "Αύριο στις 9 έχω ραντεβού με τη Μαρία στο κέντρο, υπενθύμισέ μου στις 8:30 και στείλε SMS στη Λένα."
}
```

```
RESPONSE
[
  {
    "names": ["Μαρία", "Λένα"],
    "eventDateTime": "2025-07-19T09:00:00",
    "location": "κέντρο",
    "remindBefore": "2025-07-19T08:30:00",
    "actions": [
      {
        "actionType": "sms",
        "targetNames": ["Λένα"],
        "note": "Γεια σου Λένα, αύριο έχω ραντεβού με τη Μαρία στο κέντρο."
      }
    ]
  }
]
```
### Example 4

```
REQUEST
{
"message": "Monday 10:00 conference call with Sarah and Jim, remind me 15 minutes before. Tuesday 15:00 dentist appointment, send SMS to Kate and email to the team."
}
```

```
RESPONSE
[
  {
    "names": ["Sarah", "Jim"],
    "eventDateTime": "2025-07-21T10:00:00",
    "location": "conference call",
    "remindBefore": "2025-07-21T09:45:00",
    "actions": []
  },
  {
    "names": ["Kate", "team"],
    "eventDateTime": "2025-07-22T15:00:00",
    "location": "dentist appointment",
    "remindBefore": null,
    "actions": [
      {
        "actionType": "sms",
        "targetNames": ["Kate"],
        "note": "Hi Kate, I have a dentist appointment tomorrow at 15:00."
      },
      {
        "actionType": "send_email",
        "targetNames": ["team"],
        "note": "Hello team, I will be at the dentist appointment tomorrow at 15:00."
      }
    ]
  }
]
```


## 🧑‍💻 Contributions

SchedulerAI is under active development. Feedback and feature requests are welcome!

---

