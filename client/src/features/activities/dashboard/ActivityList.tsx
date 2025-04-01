import { useActivities } from "../../../lib/hooks/useActivities";
import ActivityCard from "./ActivityCard";
import { Box } from "@mui/material";

export default function ActivityList() {
  const {activities, isLoading} = useActivities();

  if (isLoading) return <h1>Loading...</h1> 
  if (!activities) return <h1>Activities not found</h1> 

  return (
    <Box>
        {activities.map(activity => (<ActivityCard key={activity.id} activity={activity} />))}  
    </Box>
  )
}
