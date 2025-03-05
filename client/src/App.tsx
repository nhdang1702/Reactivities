import React, { useState, useEffect } from 'react';
import { Typography, ListItem, ListItemText, List } from '@mui/material';
import axios from 'axios';

function App() {
  const [activities, setActivities] = useState<Activity[]>([]);
  useEffect(() => {
    axios.get<Activity[]>('https://localhost:5001/api/activities')
      .then(response => setActivities(response.data));

    return () => {}
  }, []);
  
  return (
    <>
      <Typography variant="h1">Reactivities</Typography>
      <List>
        {activities.map(activity => (
          <ListItem key={activity.id}>
            <ListItemText primary={activity.title} secondary={activity.date} />
          </ListItem>
        ))}
      </List>
    </>
  )
}

export default App
